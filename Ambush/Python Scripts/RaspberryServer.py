import socket
import threading
import math
from enums import Components, Direction, Data, Action
import logging
import sys
import config
sys.path.insert(0, '/home/pi/Desktop/PythonScripts/EasyModbus-1.2.6/easymodbus')
from modbusClient import *

logging.basicConfig(filename='PythonLogger.log', level=logging.DEBUG)


def getIp():
    try:
        s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        s.connect(("8.8.8.8", 80))
        ip = s.getsockname()[0]
        s.close()
    except: print("No connection")
    return ip


def onInit(id, ip, Main_PC_IP, Main_PC_Port):
    message = "AR:INIT:" + str(id) + ":" + str(ip)
    # s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    # s.connect((Main_PC_IP, Main_PC_Port))
    # s.send(message)
    # data = s.recv(BUFFER_SIZE)
    # s.close()
    return message


def modBusConnect():
    # Connect to CPX
    try:
        ip = '192.168.0.199'
        port = 502
        client = ModbusClient(ip, port)
        client.connect()
        print("Connection successfully mad with Festo CPX")
        return client
    except:
        logging.warning("Error while trying to connect to modbusClient")
        print("Error while trying to connect to modbusClient, trying again")
        ip = '192.168.0.94'
        try:
            client = ModbusClient(ip, port)
            client.connect()
            print("Connection successfully mad with Festo CPX")
            return client
        except:
            logging.warning("Both CPX ips not working, please check connection")
            print("Both CPX IPs not working, please check connection")
            return None


##########################################################################
# Server and CPX settings
CPX_Port = 502
ip = '192.168.0.95'
CPX_IP = getIp()
print(str(CPX_IP))
Main_PC_IP = '10.0.0.6'
Main_PC_Port = 8080
Buffer_Size = 1024
id = config.id


# onInit(id,ip,Main_PC_IP,Main_PC_Port)
##########################################################################


def getActiveRegisters(registerSum):
    registerSum = registerSum.replace("]", "").replace("[", "")
    registerSum = int(registerSum)
    print("registerSum = " + str(registerSum))
    ActiveRegisters = []
    while (registerSum != 0):
        temp = int(math.log(registerSum, 2))
        print("temp = " + str(temp))
        ActiveRegisters.append(temp)
        print("ActiveRegisters contains : " + str(ActiveRegisters))
        registerSum -= math.pow(2, temp)
        print("RegisterSum = " + str(registerSum))
    return ActiveRegisters


def getComponentStateByRegister(activeRegisters):
    componentsStates = []  # Default component state [Middle]
    for i in range(0, 16):
        componentsStates.append(Direction.Middle.value)
    print("ACTIVE REGISTER COUNT : " + str(len(activeRegisters)))
    for i in range(0, len(activeRegisters)):
        tempReg = activeRegisters[i]
        if (tempReg % 2 == 0):
            componentsStates[tempReg] = Direction.Up.value  # Component state up
        else:
            componentsStates[tempReg - 1] = Direction.Down.value  # Component state down
    print("componentsStates   " + str(componentsStates))
    return componentsStates

def CPXStatesArrayToString():
    registersSum = getRegistersSum()
    ActiveRegisters = getActiveRegisters(str(registersSum))
    componentsStates = getComponentStateByRegister(ActiveRegisters)
    return componentsStates

def getRegistersSum():
    try:
        modbusClient = modBusConnect()
        if (not (modbusClient is None)):
            registersSum = modbusClient.read_holdingregisters(45395, 1)
            return registersSum

    except:
        print("Error while trying to read registers")
        return "FAILED"

def handleComponentGet(data):
    ########################################################
    # [0,1]--> 0 , [2,3] -->1 , [4,5] --> 2 , [6,7] --> 3 , [8,9] -->4 , [10,11] --> 5 , [12,13] --> 6 , [14,15] -->7
    # left value is on --> door state up , right value on --> door state right , none on --> door state middle
    ########################################################
    ids = data[Data.id.value]
    print("ID: " + str(ids))
    print("Attempting to read current values")
    registersSum = getRegistersSum()
    ActiveRegisters = getActiveRegisters(str(registersSum))
    componentsStates = getComponentStateByRegister(ActiveRegisters)
    # print("Before CLOSE")
    # try:
    #    modbusClient.close()
    # except:
    #    print("Unable to close modbus connection")
    print("HandleComponentsGet returned : " + str(componentsStates[int(ids)]))
    return componentsStates[int(ids)]


def getDoorReplyString(data, ans, action):
    # Build the string to be sent to the server
    did = data[Data.id.value]

    if (action == Action.Get.value):
        ans = str("AR:GET_REPLY:DR:" + str(did) + ":" + str(ans) + ":" + str(data[Data.direction.value]) + ":" + str(
            data[Data.physicalID.value]) + ":")

        return ans
    elif (action == Action.Set.value):
        return "AR:SET_REPLY:DR:" + str(did) + ":" + str(ans) + ":" + str(data[Data.direction.value]) + ":" + str(
            data[Data.physicalID.value]) + ":"


def getTargetReplyString(data, ans, action):
    # Build the string to be sent to the server
    tid = data[Data.id.value]

    if (action == Action.Get.value):
        return "AR:GET_REPLY:TR:" + str(tid) + ":" + str(ans) + ":" + str(data[Data.direction.value]) + ":" + str(
            data[Data.physicalID.value])
    elif (action == Action.Set.value):
        return "AR:SET_REPLY:TR:" + str(tid) + ":" + str(ans) + ":" + str(data[Data.direction.value]) + ":" + str(
            data[Data.physicalID.value])



def mapByID(id, direction):
    # Mapping of id-coil
    id = id % 7
    map = [33, 35, 37, 39, 41, 43, 45, 47]
    if (direction == Direction.Up.value):
        return map[id]
    elif (direction == Direction.Down.value):  # DOWN
        return map[id] - 1


def handleDoorSet(data):
    # Left coil on - UP
    # Right coil on - DOWN
    # No coil on - MIDDLE
    direction = data[Data.direction.value]
    did = data[Data.id.value]
    ans = ''
    modbusClient = modBusConnect()
    if (not (modbusClient is None)):
        try:
            # Change door state to middle
            if (direction == Direction.Middle.value):
                print("Direction received : Middle")
                modbusClient.WriteSingleCoil(mapByID(did, Direction.Up.value), 0)
                modbusClient.WriteSingleCoil(mapByID(did, Direction.Down.value), 0)
            # Change door state to Up/Down
            elif (direction == Direction.Up.value):
                print("Direction received : Up")
                modbusClient.WriteSingleCoil(mapByID(did, direction), 1)
                modbusClient.WriteSingleCoil(mapByID(did, Direction.Down.value), 0)
            elif (direction == Direction.Down.value):
                print("Direction received : Down")
                modbusClient.WriteSingleCoil(mapByID(did, direction), 1)
                modbusClient.WriteSingleCoil(mapByID(did, Direction.Up.value), 0)
            ans = 'SUCCESS'
        except:
            ans = 'FAILED'
            logging.warning("Failed to set coils AT handleSet RaspberryServer.py")
            modbusClient.close()
    return ans


def ProcessRequest(data):
    # Get data from string
    data = data.split(":")
    print("Action: " + str(data[Data.action.value]))
    if (str(data[Data.action.value]) == str('BROD')):
        print("Broadcast message received. Returning answer")
        return "AR:BROD_REPLY:" + str(id) + ":" + str(ip) + "/r/n"

    if (str(data[Data.action.value]) == str(Action.Get.value)):
        print("Getting data ....")
        if (str(data[Data.component.value]) == str(Components.Door.value)):
            print("Getting door states")
            ans = handleComponentGet(data)
            print("Answer : " + str(ans))
            if (str(ans) != str("FAILED")):
                return getDoorReplyString(data, ans, Action.Get.value)
            return "AR:GET_REPLY:FAILED:TO:READ"

        if (str(data[Data.component.value]) == str(Components.Target.value)):
            print("Getting target state")
            ans = handleComponentGet(data)
            print("Answer : " + str(ans))
            return getTargetReplyString(data, ans, Action.Get.value)
        if (str(data[Data.component.value]) == str(Components.CPX.value)):
            return CPXStatesArrayToString()

    # Set
    if (str(data[Data.action.value]) == str(Action.Set.value)):
        if (str(data[Data.component.value]) == str(Components.Door.value)):
            print("Setting door state")
            ans = handleDoorSet(data)
            print("Answer : " + str(ans))
            return getDoorReplyString(data, ans, Action.Set.value)

        if (str(data[Data.component.value]) == str(Components.Target.value)):
            print("Setting target's state")

            ####TODO ---> handleTargetSet function once states are known
            ans = handleTargetSet()
            print("Answer : " + str(ans))
            return getTargetReplyString(data, ans, Action.Set.value)
    if (str(data[Data.action.value]) == str("INIT")):
        return onInit(id, ip, Main_PC_IP, Main_PC_Port)



# Threaded server class to accept and process clients requests
class ThreadedServer(object):
    def __init__(self, host, port):
        self.host = host
        self.port = port
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.sock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        self.sock.bind((self.host, self.port))

    def listen(self):
        self.sock.listen(5)
        while True:
            client, address = self.sock.accept()
            client.settimeout(60)
            threading.Thread(target=self.listenToClient, args=(client, address)).start()

    def listenToClient(self, client, address):
        size = 1024
        while True:
            try:
                print("Client connected")
                data = client.recv(size)
                if data:
                    # Set the response to echo back the recieved data
                    print("Incoming data : " + data)
                    ans = ProcessRequest(data)

                    print("Answer returned : " + ans)
                    client.send(ans)
                else:
                    raise socket.error('Client disconnected')
            except:
                client.close()
                return False


if __name__ == "__main__":
    port_num = 8085
ThreadedServer(CPX_IP, port_num).listen()
