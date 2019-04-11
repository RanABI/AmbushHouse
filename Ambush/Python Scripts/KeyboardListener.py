from threading import Thread, Condition
from pynput.keyboard import Key, Listener
import socket
import time
import random
import sys

RPI_ID =  sys.argv[1]
sensorNum=[]
hitStrength =[]
queue = []
condition = Condition()

#Server configuration
BUFFER_SIZE = 1024
SERVER_IP = '10.0.0.1'
SERVER_PORT = 8085
def rasbToServer(MESSAGE):
    # Server configuration
    BUFFER_SIZE = 1024
    SERVER_IP = '10.0.0.1'
    SERVER_PORT = 8085

    #Connect to server2s= through socket using given ip and port
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.connect((SERVER_IP,SERVER_PORT))
    #send data message to server
    s.send(MESSAGE.encode())
    #receive server's answer
    data = s.recv(BUFFER_SIZE)
    #close the connection
    s.close()


def buildString(sensorNum,hitStrength):
    print("AR:REPLY:TR:" + sensorNum + ":" + hitStrength)
    rasbToServer("AR:HIT:TR:" + sensorNum + ":" + hitStrength + RPI_ID)
def normalize(key):
    return str((ord(key) - 97)/(122-97)*10)

def on_press(key):
    #functions as a producer that adds works for consumer
    global queue
    condition.acquire()
    input = str(key.char)
    queue.append(input)
    condition.notify()
    condition.release()



class ConsumerThread(Thread):
    def run(self):
        global queue
        global sensorNum
        global hitStrength
        while True:
            condition.acquire()
            if not queue:
                #wait for producer to add tasks
                condition.wait()
            else:
                num = queue.pop(0)
                #Add hit strength and sensor number to respected lists
                if(num.isalpha()):
                    hitStrength.append(normalize(num))
                elif(num.isdigit()):
                    sensorNum.append(num)
                if sensorNum and hitStrength:
                    sens = sensorNum.pop(0)
                    strength = hitStrength.pop(0)
                    buildString(str(sens),str(strength))
                condition.notify()
                condition.release()


ConsumerThread().start()


with Listener(
        on_press=on_press,
        ) as listener:
    listener.join()
