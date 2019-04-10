from enum import Enum
class Data(Enum):
    action = 1
    component = 2
    id = 3
    direction = 4
    physicalID = 5
class Direction(Enum):
    Up=1
    Middle = 2
    Down = 3
class Components(Enum):
    Door='DR'
    Target = 'TR'
    Sensor = 'SN'
    CPX= 'CPX'

class Action(Enum):
    Get = 'GET'
    Set = 'SET'
    Brod = 'BROD'
