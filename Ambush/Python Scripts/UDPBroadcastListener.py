import socket
import threading
import sys
import config
id = config.id
def getIp():
    s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    s.connect(("8.8.8.8",80))
    ip = s.getsockname()[0]
    s.close()
    return str(ip)

sock = socket.socket(socket.AF_INET,socket.SOCK_DGRAM)
server_address = ('',9434)
sock.bind(server_address)

response = 'AR:BROD_REPLY:' +str(id)+":" + str(getIp())
print(getIp())
while True:
    
    data,address = sock.recvfrom(4096)
    print("NOT HERE")
    data = str(data.decode('UTF-8'))
    print(data)
    if data == 'AR:BROD':
        sent = sock.sendto(response.encode(),address)