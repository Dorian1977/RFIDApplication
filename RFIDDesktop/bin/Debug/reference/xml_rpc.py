import sys
path = sys.argv[0]  #1 argument given is a string for the path
sys.path.append(path)
import xmlrpclib

admin = 2
RFIDConnected = False
RFIDTagReady = False 
#workorder_product_code = '' #SU-0067
RFIDEPCTagInfo = ''#'PS999999999999999999=SU-00670521' #max 18 digits, ink type, expire date
uid = 0
password = ""
db = ""
models = 0
workorder_product_id = ""
workorder_id = ""
workorder_production_id = ""

class XmlRpc:    
    def isRFIDConnected(self, RFIDStatus):
        global RFIDConnected
        RFIDConnected = RFIDStatus
        print('RFID status is connected? ', RFIDStatus)

    #1. display ready to read and write
    def readyToUpdateTag(self):
        print('ready to update tag: ', RFIDTagReady)
        return RFIDTagReady

    #2. get RFID tag, update the RFID data
    def readRFIDTagID(self, RFIDEPCTagID):
        global RFIDEPCTagInfo
        RFIDEPCTagInfo = ''
        if RFIDEPCTagID.find('PS') != -1:
            try:
                #prepare data to write
                # Search for Work Order which the user is currently working on
                workorder_rec = models.execute_kw(db, uid, password, 'mrp.workorder', 'search_read', [[['state','=','progress']]], {'fields': ['name', 'product_id', 'production_id', 'is_user_working', 'working_user_ids'], 'limit': 10})
                
                global workorder_production_id
                global workorder_product_id
                global workorder_id

                for workorder in workorder_rec:
                    workorder_id = workorder.get('id')
                    workorder_product_id = workorder.get('product_id')
                    workorder_production_id = workorder.get('production_id')
                    workorder_working = workorder.get('is_user_working')
                    workorder_user = workorder.get('working_user_ids')

                    if (uid in workorder_user and workorder_working == True):
                        # Search for Part Number of the Work Order Product
                        workorder_rec = models.execute_kw(db, uid, password, 'product.product', 'search_read', [[['id','=',workorder_product_id[0]]]], {'fields': ['engineering_code']})
                        workorder_product_code = workorder_rec[0].get('engineering_code')
                        RFIDEPCTagInfo = RFIDEPCTagID + '=' + workorder_product_code                    
                        return True
            except (Exception):
                return False
        return False
        
    def readProductId(self):
        return workorder_product_id
    
    def readProductionId(self):
        global workorder_production_id
        global workorder_product_id
        global workorder_id

        try:
            #prepare data to write
            # Search for Work Order which the user is currently working on
            workorder_rec = models.execute_kw(db, uid, password, 'mrp.workorder', 'search_read', [[['state','=','progress']]], {'fields': ['name', 'product_id', 'production_id', 'is_user_working', 'working_user_ids'], 'limit': 10})
            workOrderCount = 0

            for workorder in workorder_rec:                               
                workorder_working = workorder.get('is_user_working')
                workorder_user = workorder.get('working_user_ids')
                if uid in workorder_user and workorder_working == True:
                    workOrderCount += 1
                    workorder_product_id = workorder.get('product_id')
                    workorder_production_id = workorder.get('production_id')
                    workorder_id = workorder.get('id')

        except (Exception):
                return 0

        if workOrderCount > 1:
            return workOrderCount

        return workorder_production_id
        
    #3. update RFID tag data, when RFIDEPCTagInfo is updated 
    def updateTag(self):
        print('write tag: ', RFIDEPCTagInfo)
        return RFIDEPCTagInfo
    
    #4. get RFID tag updated successful
    def writeRFIDTagSuccessful(self, RFIDEPCTagInfo):       
        #if RFIDTagstatus == True:  
        try:
            serial_number_id = models.execute_kw(db, uid, password, 'stock.production.lot', 'create',[{'name': RFIDEPCTagInfo, 'product_id': workorder_product_id[0]}])
               
            #Write serial number to Work Order
            models.execute_kw(db, uid, password, 'mrp.workorder', 'write',[[workorder_id],{'final_lot_id': serial_number_id, 'x_serial_number_id': serial_number_id}])
                
            #Display the Serial Number on Screen. Need a onchange function???
            #models.execute_kw(db, uid, password, 'mrp.workorder', 'update_serial_number', [int(workorder_id)])
            #models.execute_kw(db, uid, password, 'update_serial_number', 'write', [{'id': workorder_id, 'number': serial_number_id}])
        except (Exception):
            return False
        return True
        #return False
        
    #Get RFID tag number       
    def getRFIDNumber(self):
        serial_rec = models.execute_kw(db, uid, password, 'ir.sequence', 'search_read', [[['code','=','rfid']]], {'fields': ['number_next_actual']})
        global serial_id
        serial_id = serial_rec[0].get('id')
        serial_number = serial_rec[0].get('number_next_actual')
        return serial_number
        
    #move to next number + Increase by 1 
    def getNextRFIDNumber(self, serial_number):
        next_serial_number = serial_number + 1
        id = models.execute_kw(db, uid, password, 'ir.sequence', 'write',[[serial_id], {'number_next_actual': next_serial_number}])
    
    def getUserName(self):
        try:
            user_info = models.execute_kw(db, uid, password, 'res.users', 'search_read', [[['id','=',uid]]], {'fields': ['name']})
            print('My name is', user_info[0].get('name'))
            return user_info[0].get('name')
        except (Exception):
            return ""

    #login is simulate the overall process, need to simplify
    def login(self, url, _db, username, _password): #db, username, password):
        #Log in Odoo
        common = xmlrpclib.ServerProxy('{}/xmlrpc/common'.format(url))

        global uid
        uid = common.authenticate(_db, username, _password, {})
        
        global db
        db = _db
                    
        global password
        password = _password

        global models
        models = xmlrpclib.ServerProxy('{}/xmlrpc/object'.format(url))

        #(Dorian)Check if RFID reader is connect, raise message if not connected
        if RFIDConnected == False:
            return False

        #For Dorian:
        #Prompt User to put ink bottle on scanner
        global RFIDTagReady
        RFIDTagReady = True
        
        group_rec = models.execute_kw(db, uid, password, 'res.groups', 'search_read', [[['name','=','RFID Tag Creation']]], {'fields': ['users']})           
        list_of_user = group_rec[0].get('users')
        if uid in list_of_user:
            #print('Has privilege')
            return True
        #print('Dont have privilege')
        return False           

        #(Jason)Update the number on screen
        #(Jason)Move to next one
        
