import datetime

def getType():
    return  [{ "id": "TankMeasurement", "type": "object", "classification": "dynamic", "properties": { "Time": { "format": "date-time", "type": "string", "isindex": True }, "Pressure": { "type": "number", "name": "Tank Pressure", "description": "Tank Pressure in Pa" }, "Temperature": { "type": "number", "name": "Tank Temperature", "description": "Tank Temperature in K" } } }]

def getContainer():
    return  [{"id": "Tank1Measurements", "typeid": "TankMeasurement", "typeVersion": "1.0.0.0"}]

def getData(pressure, temperature):
    return  [{ "containerid": "Tank1Measurements", "values": [{ "Time": datetime.datetime.utcnow().isoformat(), "Pressure": pressure, "Temperature": temperature}] }]
