
#ifndef    _LSOCK_         //STDIO_H 가 선언되어 있지 않다면
#define    _LSOCK_   

#define WIN32_LEAN_AND_MEAN
#include <winsock2.h>
#include <ws2tcpip.h>
#include <Windows.h>
#include <iostream>
#include <thread>

#pragma comment (lib, "Ws2_32.lib")

#define BUFLEN 2
#define PORT "555"

class L_sock{
private:

	static L_sock* instance;

	WSADATA wsaData;
	int iResult;

	SOCKET ListenSocket = INVALID_SOCKET;
	SOCKET ClientSocket = INVALID_SOCKET;

	struct addrinfo *result = NULL;
	struct addrinfo hints;

	int iSendResult;
	char recvbuf[BUFLEN];
	char sendbuf[BUFLEN];

	int recvbuflen = BUFLEN;
	bool Connect_Succeed;
	bool Csharp_down;
	

public:

	bool Server_Ready;

	static L_sock* GetInstance() {
		if (instance == NULL)
			instance = new L_sock();
		return instance;
	}


	void sock_connect();

	void Label_trans(char message, int Label);

	void sock_close();

	bool get_Connect_Succeed();
	void set_Connect_Succeed(bool value);

	bool get_Csharp_down();
	void set_Csharp_down(bool value);

	void Transe_message(char message);

	void Recv_message();
};
L_sock* L_sock::instance = NULL;

void L_sock::sock_connect(){

	printf("SERVER START!\n");

	iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);
	if (iResult != 0) {
		printf("WSAStartup failed with error: %d\n", iResult);
		return;
	}

	ZeroMemory(&hints, sizeof(hints));
	hints.ai_family = AF_INET;
	hints.ai_socktype = SOCK_STREAM;
	hints.ai_protocol = IPPROTO_TCP;
	hints.ai_flags = AI_PASSIVE;

	// Resolve the server address and port
	iResult = getaddrinfo(NULL, PORT, &hints, &result);
	if (iResult != 0) {
		printf("getaddrinfo failed with error: %d\n", iResult);
		WSACleanup();
		return;
	}

	// Create a SOCKET for connecting to server
	ListenSocket = socket(result->ai_family, result->ai_socktype, result->ai_protocol);
	if (ListenSocket == INVALID_SOCKET) {
		printf("socket failed with error: %ld\n", WSAGetLastError());
		freeaddrinfo(result);
		WSACleanup();
		return;
	}
	
	// Setup the TCP listening socket
	//bind(ListenSocket, result->ai_addr, (int)result->ai_addrlen);
	 bind(ListenSocket, result->ai_addr, (int)result->ai_addrlen);
	//if (iResult == SOCKET_ERROR) {
	//	printf("bind failed with error: %d\n", WSAGetLastError());
	//	freeaddrinfo(result);
	//	closesocket(ListenSocket);
	//	WSACleanup();
	//	return;
	//}

	freeaddrinfo(result);

	iResult = listen(ListenSocket, SOMAXCONN);
	if (iResult == SOCKET_ERROR) {
		printf("listen failed with error: %d\n", WSAGetLastError());
		closesocket(ListenSocket);
		WSACleanup();
		return;
	}


	L_sock::GetInstance()->Server_Ready = true;
	//cout <<"test="<< GetInstance()->Server_Ready << endl;

	//printf("Server_Ready Finish!!");
	// Accept a client socket
	ClientSocket = accept(ListenSocket, NULL, NULL);
	if (ClientSocket == INVALID_SOCKET) {
		printf("accept failed with error: %d\n", WSAGetLastError());
		closesocket(ListenSocket);
		WSACleanup();
		return;
	}
	else if (ClientSocket != -1){
		Connect_Succeed = true;	
	}

	while (Connect_Succeed == true){
		Recv_message();
		if (recvbuf[0] == 5)
			set_Csharp_down(true);
		
	}
}


void L_sock::Recv_message(){
	
	recv(ClientSocket, recvbuf, BUFLEN, 0);
};

void L_sock::Label_trans(char message, int Label) {
	sendbuf[0] = message;
	sendbuf[1] = Label;

	iSendResult = send(ClientSocket, sendbuf, BUFLEN, 0);
	if (iSendResult == SOCKET_ERROR) {
		printf("send failed with error: %d\n", WSAGetLastError());
		closesocket(ClientSocket);
		WSACleanup();
		return;
	}
	printf("Bytes sent1: %d\n", iSendResult);
}

void L_sock::Transe_message(char message) {
	sendbuf[0] = message;

	iSendResult = send(ClientSocket, sendbuf, BUFLEN, 0);
	if (iSendResult == SOCKET_ERROR) {
		printf("send failed with error: %d\n", WSAGetLastError());
		closesocket(ClientSocket);
		WSACleanup();
		return;
	}
	printf("Bytes sent2: %d\n", iSendResult);

}

void L_sock::sock_close() {
	closesocket(ClientSocket);
	WSACleanup();
}

bool L_sock::get_Connect_Succeed(){
	return Connect_Succeed;
}
void L_sock::set_Connect_Succeed(bool value){
	Connect_Succeed = value;
}

bool L_sock::get_Csharp_down(){
	return Csharp_down;
};
void L_sock::set_Csharp_down(bool value){
	Csharp_down = value;
};
#endif