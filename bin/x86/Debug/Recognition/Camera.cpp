

#include "L_sock.h"
#include "Camera.h"


using namespace cv;
using namespace std;
int START = 0;
const double Camera::TICK_FREQUENCY = getTickFrequency();
const String const Camera::TITLE = "Face Recognize";
const String const Camera::CASCADE_FILE = ".\\haarcascade_frontalface_alt2.xml";
const char*  C_SHop_Path = "start /D \"..\\..\\\" Project_Worker.exe";
 char* att_faces_path = "C:\\PC_att_faces";
 char* csv_path = ".\\csv.ext";
 char* create_csv_path = ".\\create_csv.py";

const double Threshold = 3300;

#define Ls_face_detect_start 1
#define Ls_detection_end 2
#define Ls_Label 3
#define Ls_exit 4
#define Ls_csharp_exit 5



static void read_csv(const string& filename, vector<Mat>& images, vector<int>& labels, char separator = ';') {
	std::ifstream file(filename.c_str(), ifstream::in);
	if (!file) {
		string error_message = "No valid input file was given, please check the given filename.";
		CV_Error(CV_StsBadArg, error_message);
	}
	string line, path, classlabel;
	images.clear();
	labels.clear();
	while (getline(file, line)) {
		stringstream liness(line);
		getline(liness, path, separator);
		getline(liness, classlabel);
		if (!path.empty() && !classlabel.empty()) {
			images.push_back(imread(path, 0));
			labels.push_back(atoi(classlabel.c_str()));
		}
	}
}


void create_csv(){
	//csv���� ����     ����: python 'create_csv.py���' 'att_faces���' > 'csv.ext���������� ���'

	char path[255];
	sprintf_s(path,sizeof(path),"python %s %s > %s",create_csv_path,att_faces_path,csv_path);

	system(path);
	
}

void Sock_start(){
	L_sock::GetInstance()->sock_connect();

}
void Sock_close(){
	L_sock::GetInstance()->sock_close();
}
void Sock_Label(char message, int Label){
	L_sock::GetInstance()->Label_trans(message, Label);
}
Camera::Camera() {
	L_sock::GetInstance()->set_Connect_Succeed(false);
	L_sock::GetInstance()->set_Csharp_down(false);
	L_sock::GetInstance()->Server_Ready = false;

	std::thread Connect_thread(Sock_start);
	Connect_thread.detach();



	while (true)
	{
		if (L_sock::GetInstance()->Server_Ready == true){	
			
			system(C_SHop_Path);
			
			L_sock::GetInstance()->Server_Ready = false;
		}

		if (L_sock::GetInstance()->get_Connect_Succeed() == true) {

			break;
		}
	}
	cout << "Starting show cam.." << endl;
	Init();


}

void Camera::Init() {
	char cmd[255];
	sprintf_s(cmd,sizeof(cmd), "mkdir %s", att_faces_path);
	system(cmd);

	cap = VideoCapture(0);

	if (!cap.isOpened()) {
		cout << "cam is can't open" << endl;
		return;
	}

	if (!path_xml.load(CASCADE_FILE)) {
		cout << " Error loading file" << endl;
		return;
	}

	namedWindow(TITLE, 0);
	resizeWindow(TITLE, 300, 200);
	moveWindow(TITLE, 0, 0);
}

Rect Camera::face() const
{
	Rect faceRect = trackingface;
	faceRect.x = (int)(faceRect.x / scale);
	faceRect.y = (int)(faceRect.y / scale);
	faceRect.width = (int)(faceRect.width / scale);
	faceRect.height = (int)(faceRect.height / scale);
	return faceRect;
}

void Camera::FaceDetect() {    //Camera.cpp �߿��� ���ϸ��� ����Ǵ°�
	//c#�� ����Ǿ����� check



	////////////////////�νļҽ�///////////////////////////////
	string box_text;
	//double Threshold = 2200;
	int waitnum;
	//csv���� ����
	create_csv();

	

	vector<Mat> images;
	vector<int> labels;

	try {
		read_csv(csv_path, images, labels);
	}
	catch (cv::Exception& e) {
		cerr << "Error opening file \"" << csv_path << "\". Reason: " << e.msg << endl;

		exit(1);
	}

	int im_width = images[0].cols;  //92
	int im_height = images[0].rows;  //112

	Ptr<FaceRecognizer> model = createEigenFaceRecognizer(80, Threshold);  // threshold ĸ����ǿ��� 3300�߾���  //2500���Ͽ����� �νķ� ������
	model->train(images, labels);

	///////////////////////////////////////////////////////////

	int prediction_start = 0; //predict �� ������ �˸��� ���� 
	while (L_sock::GetInstance()->get_Csharp_down()==false) {

		Mat frame;
		cap >> frame;
		scale = (double)std::min(width, frame.cols) / frame.cols;
		original = frame.clone();
		Mat gray;
		cvtColor(original, gray, CV_BGR2GRAY); //�׷��̷� ��ȯ

		equalizeHist(gray, gray); //��Ȱȭ

		Size resizedFrameSize = Size((int)(scale*gray.cols), (int)(scale*gray.rows));
		Mat resizedFrame;
		resize(gray, resizedFrame, resizedFrameSize);


		if (!found_Face) {
			detectFaceAllSizes(resizedFrame);

			if (!START) {
				imshow(TITLE, original);
				continue;
			}
		}
		else {		//Frame size (640,480)
			detectFaceAroundRoi(resizedFrame);

			if (templateMatch_Run)
				if (face().x <= 0 || (face().x + face().width) >= frame.cols || face().y <= 0 || (face().y + face().height) >= frame.rows) { //���� ��������� �ȿ������� ����
					cout << "���� �νĹ����� ������ϴ�" << endl;
					found_Face = 0;
					prediction_start = 0;
					//���� ȭ����� ���� ������ ���� �������
					if (L_sock::GetInstance()->get_Connect_Succeed() == true)
						L_sock::GetInstance()->Transe_message(Ls_detection_end);
					templateMatch_MaxDuration = 2;
					continue;
				}
				else {
					detectFacesTemplateMatching(resizedFrame);
				}
		}
		if (!(face().x <= 0 || (face().x + face().width) >= frame.cols || face().y <= 0 || (face().y + face().height) >= frame.rows)) {  //���� ��������� �ȿ������� ����
			rectangle(frame, face(), Scalar(0, 255, 0), 3);

			////////////////////�νļҽ�///////////////////////////////

			Mat reface = gray(face()); //�׷��� �����Ϸ� ��ȯ

			Mat face_resized;

			cv::resize(reface, face_resized, Size(im_width, im_height), 1.0, 1.0, INTER_CUBIC);  //�׷��̽����� ��ȯ�� ���� 92x112�� resize

			//prediction ���� �Ǵ°�
			int prediction = model->predict(face_resized);
			//prediction value default -1

			Point lu2(face().x + 1, face().y + 1);
			Point rd(face().x + face().width - 1, face().y + face().height - 1);
			Rect roi(lu2, rd);


			if (prediction == -1)
				box_text = "";
			else
				box_text = to_string(prediction);

			putText(frame, box_text, Point(50, 50), FONT_HERSHEY_PLAIN, 1.0, CV_RGB(0, 0, 255), 2.0); //putText �ѱ������� �ȵǳ�??



			if (prediction_start == 1) //���� �νĵǾ��� �ι�° �ν��̶��
				prediction_start = 2;	 //2�� 2�� �̻� ���� �Ǿ��� �Ҹ�

			if (prediction != -1 && prediction_start == 0) {//���� �νĵǾ��� ù�ν��̶��
				prediction_start = 1;//1�� 1�� ����Ǿ��� �Ҹ�		

				//   ****************���� ù�νĽ� ����� �͵� �������*************************
				//C#���α׷��� Label���� �����ϴ� ��
				if (L_sock::GetInstance()->get_Connect_Succeed() == true)
					L_sock::GetInstance()->Label_trans(Ls_Label, prediction);
				templateMatch_MaxDuration = 60;
				/////////////////////////////////////
			}
		}
		imshow(TITLE, frame);

		if (waitnum = waitKey(30)) {

			if (waitnum == 27) { //esc������ ����
				//�������� close �ڵ�
				if (L_sock::GetInstance()->get_Connect_Succeed() == true){
					L_sock::GetInstance()->Transe_message(Ls_exit);
					L_sock::GetInstance()->sock_close();
				}
				break;
			}
		}

	}
}

Rect Camera::doubleRectSize(const Rect &inputRect, const Rect &frameSize) const
{
	Rect outputRect;
	// Double rect size
	outputRect.width = inputRect.width * 2;
	outputRect.height = inputRect.height * 2;

	// Center rect around original center
	outputRect.x = inputRect.x - inputRect.width / 2;
	outputRect.y = inputRect.y - inputRect.height / 2;

	// Handle edge cases
	if (outputRect.x < frameSize.x) {
		outputRect.width += outputRect.x;
		outputRect.x = frameSize.x;
	}
	if (outputRect.y < frameSize.y) {
		outputRect.height += outputRect.y;
		outputRect.y = frameSize.y;
	}

	if (outputRect.x + outputRect.width > frameSize.width) {
		outputRect.width = frameSize.width - outputRect.x;
	}
	if (outputRect.y + outputRect.height > frameSize.height) {
		outputRect.height = frameSize.height - outputRect.y;
	}

	return outputRect;
}


Rect Camera::biggestFace(vector<Rect> &faces) const
{
	Rect *biggest = &faces[0];
	for (auto &face : faces) {
		if (face.area() < biggest->area())
			biggest = &face;
	}
	return *biggest;
}

/*
* Face template is small patch in the middle of detected face.
*/
Mat Camera::getFaceTemplate(const Mat &frame, Rect face)  //�� �Լ��� �ּ�ó���ϸ� ���� ��������� ������ �����ʴ´� ����������� �����ϴµ� � ������ �ִµ�
{   //���⼭ face�� rect�� �ڷ�
	face.x += face.width / 4;
	face.y += face.height / 4;
	face.width /= 2;
	face.height /= 2;

	Mat faceTemplate = frame(face).clone();
	return faceTemplate;
}

void Camera::detectFaceAllSizes(const Mat &frame)
{
	// Minimum face size is 1/5th of screen height
	// Maximum face size is 2/3rds of screen height
	path_xml.detectMultiScale(frame, faces, 1.1, 3, 0,  //faces���� ������ �󱼵��� ��� ���ִµ� ���⼭�� ����ū���� �������� �ϱ����� biggestface�� ����
		Size(frame.rows / 4, frame.rows / 4),
		Size(frame.rows * 2 / 3, frame.rows * 2 / 3));

	if (faces.empty())
		return;

	found_Face = true;
	START = 1;
	// Locate biggest face
	trackingface = biggestFace(faces);

	// Copy face template
	face_Template = getFaceTemplate(frame, trackingface);

	// Calculate roi
	face_ROI = doubleRectSize(trackingface, Rect(0, 0, frame.cols, frame.rows));

	if (L_sock::GetInstance()->get_Connect_Succeed() == true){
		printf("���ӵ�");
		L_sock::GetInstance()->Transe_message(Ls_face_detect_start);
	}

}

void Camera::detectFaceAroundRoi(const Mat &frame)
{
	// Detect faces sized +/-20% off biggest face in previous search
	path_xml.detectMultiScale(frame(face_ROI), faces, 1.1, 3, 0,
		Size(trackingface.width * 8 / 10, trackingface.height * 8 / 10),
		Size(trackingface.width * 12 / 10, trackingface.width * 12 / 10));

	if (faces.empty())
	{
		// Activate template matching if not already started and start timer
		templateMatch_Run = true;
		if (templateMatch_Start == 0)
			templateMatch_Start = getTickCount();
		return;
	}

	// Turn off template matching if running and reset timer
	templateMatch_Run = false;
	templateMatch_Cur = templateMatch_Start = 0;

	// Get detected face
	trackingface = biggestFace(faces);

	// Add roi offset to face
	trackingface.x += face_ROI.x;
	trackingface.y += face_ROI.y;

	// Get face template
	face_Template = getFaceTemplate(frame, trackingface);

	// Calculate roi
	face_ROI = doubleRectSize(trackingface, Rect(0, 0, frame.cols, frame.rows));

}

void Camera::detectFacesTemplateMatching(const Mat &frame)
{
	// Calculate duration of template matching
	templateMatch_Cur = getTickCount();
	double duration = (double)(templateMatch_Cur - templateMatch_Start) / TICK_FREQUENCY;

	// If template matching lasts for more than 2 seconds face is possibly lost
	// so disable it and redetect using cascades
	if (duration > templateMatch_MaxDuration) {
		found_Face = false;
		templateMatch_Run = false;
		templateMatch_Start = templateMatch_Cur = 0;
		return;
	}

	// Template matching with last known face 
	//cv::matchTemplate(frame(m_faceRoi), m_faceTemplate, m_matchingResult, CV_TM_CCOEFF);
	matchTemplate(frame(face_ROI), face_Template, result, CV_TM_SQDIFF_NORMED);
	normalize(result, result, 0, 1, NORM_MINMAX, -1, cv::Mat());
	double min, max;
	Point minLoc, maxLoc;
	minMaxLoc(result, &min, &max, &minLoc, &maxLoc);

	// Add roi offset to face position
	minLoc.x += face_ROI.x;
	minLoc.y += face_ROI.y;

	// Get detected face
	//m_trackedFace = cv::Rect(maxLoc.x, maxLoc.y, m_trackedFace.width, m_trackedFace.height);
	trackingface = Rect(minLoc.x, minLoc.y, face_Template.cols, face_Template.rows);
	trackingface = doubleRectSize(trackingface, Rect(0, 0, frame.cols, frame.rows));

	// Get new face template
	face_Template = getFaceTemplate(frame, trackingface);

	// Calculate face roi
	face_ROI = doubleRectSize(trackingface, Rect(0, 0, frame.cols, frame.rows));


}




