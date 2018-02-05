import sys
import os.path

BASE_PATH="C:\PC_att_faces" # C:\PC_att_faces
SEPARATOR=";"

label = 0
dirNames = ()
for dirname, dirnames, filenames in os.walk(BASE_PATH): #dirname = C:\PC_att_faces, dirnames = 1~11
    dirNames = list(dirNames)    
    for i in dirnames:
	index = 1
        
	dirNames.insert(index,int(i))
 	index += 1
    dirNames.sort()
    dirNames = tuple(dirNames)
    for subdirname in dirNames:
        subject_path = os.path.join(dirname, str(subdirname))
        for filename in os.listdir(subject_path):
            abs_path = "%s/%s" % (subject_path, filename)
            print "%s%s%d" % (abs_path, SEPARATOR, label)
        label = label + 1