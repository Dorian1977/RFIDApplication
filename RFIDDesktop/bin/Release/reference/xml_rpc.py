import sys
path = sys.argv[0]  #1 argument given is a string for the path
sys.path.append(path)

import xmlrpclib
from datetime import datetime 
from datetime import timedelta

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
workorder_list = ""

class XmlRpc:    
    def isRFIDConnected(self, RFIDStatus):
        global RFIDConnected
        RFIDConnected = RFIDStatus
        print('RFID status is connected? ', RFIDStatus)

    #1. display ready to read and write
    def readyToUpdateTag(self):
        print('ready to update tag: ', RFIDTagReady)
        return RFIDTagReady

    ##2. get RFID tag, update the RFID data
    #def readRFIDTagID(self, RFIDEPCTagID):
    #    global RFIDEPCTagInfo
    #    RFIDEPCTagInfo = ''
    #    if RFIDEPCTagID.find('PS') != -1:
    #        try:
    #            #prepare data to write
    #            # Search for Work Order which the user is currently working on
    #            workorder_rec = models.execute_kw(db, uid, password, 'mrp.workorder', 'search_read', [[['state','=','progress']]], {'fields': ['name', 'product_id', 'production_id', 'is_user_working', 'working_user_ids'], 'limit': 10})
                
    #            global workorder_production_id
    #            global workorder_product_id
    #            global workorder_id

    #            for workorder in workorder_rec:
    #                workorder_id = workorder.get('id')
    #                workorder_product_id = workorder.get('product_id')
    #                workorder_production_id = workorder.get('production_id')
    #                workorder_working = workorder.get('is_user_working')
    #                workorder_user = workorder.get('working_user_ids')

    #                if (uid in workorder_user and workorder_working == True):
    #                    # Search for Part Number of the Work Order Product
    #                    workorder_rec = models.execute_kw(db, uid, password, 'product.product', 'search_read', [[['id','=',workorder_product_id[0]]]], {'fields': ['engineering_code']})
    #                    workorder_product_code = workorder_rec[0].get('engineering_code')
    #                    RFIDEPCTagInfo = RFIDEPCTagID + '=' + workorder_product_code                    
    #                    return RFIDEPCTagInfo
    #        except (Exception):
    #            return ''
    #    return ''

    def readWorkOrderList(self):
        global workorder_list
        workorder_list = models.execute_kw(db, uid, password, 'mrp.workorder', 'search_read', [[('state','!=','done')]], {'fields': ['id', 'name', 'product_id', 'production_id']})
        workorders = []
        try:
            for workorder in workorder_list:
                _workorder_product_id = workorder.get('product_id')
                _workorder_production_id = workorder.get('production_id')
                workorders.append(_workorder_production_id)
        except (Exception):
            return workorders
        return workorders

    def getProductInfo(self, productionInfo):
        global workorder_id
        global workorder_product_id

        producInfo = []
        for workorder in workorder_list:
            workorder_id = workorder.get('id')
            _workorder_product_id = workorder.get('product_id')
            _workorder_production_id = workorder.get('production_id')

            if productionInfo == _workorder_production_id[1]:
                workorder_rec = models.execute_kw(db, uid, password, 'mrp.workorder', 'read',[workorder_id],{'fields': ['product_id', 'production_id', 'qty_remaining', 'qty_produced']})
                workorder_product_id = workorder_rec[0].get('product_id')
                workorder_production_id = workorder_rec[0].get('production_id')
                product_rec = models.execute_kw(db, uid, password, 'product.product', 'search_read', [[['id','=',workorder_product_id[0]]]], {'fields': ['engineering_code']})
                workorder_product_code = product_rec[0].get('engineering_code')
                producInfo.append(workorder_product_id)
                producInfo.append(workorder_product_code)
                return  producInfo
        return ''
    #def readProductId(self):
    #    return workorder_product_id
    
    #def readProductionId(self):
    #    global workorder_production_id
    #    global workorder_product_id
    #    global workorder_id

    #    try:
    #        #prepare data to write
    #        # Search for Work Order which the user is currently working on
    #        workorder_rec = models.execute_kw(db, uid, password, 'mrp.workorder', 'search_read', [[['state','=','progress']]], {'fields': ['name', 'product_id', 'production_id', 'is_user_working', 'working_user_ids'], 'limit': 10})
    #        workOrderCount = 0

    #        for workorder in workorder_rec:                               
    #            workorder_working = workorder.get('is_user_working')
    #            workorder_user = workorder.get('working_user_ids')
    #            if uid in workorder_user and workorder_working == True:
    #                workOrderCount += 1
    #                workorder_product_id = workorder.get('product_id')
    #                workorder_production_id = workorder.get('production_id')
    #                workorder_id = workorder.get('id')

    #    except (Exception):
    #            return 0

    #    if workOrderCount > 1:
    #        return workOrderCount

    #    return workorder_production_id
        
    #3. update RFID tag data, when RFIDEPCTagInfo is updated 
    #def updateTag(self):
    #    print('write tag: ', RFIDEPCTagInfo)
    #    return RFIDEPCTagInfo
    
    #4. get RFID tag updated successful
    def getProduced(self):
        workorder_status = models.execute_kw(db, uid, password, 'mrp.workorder', 'read',[workorder_id],{'fields': ['qty_remaining', 'qty_produced']})
        return workorder_status[0].get('qty_produced')

    def getTotal(self):
        workorder_status = models.execute_kw(db, uid, password, 'mrp.workorder', 'read',[workorder_id],{'fields': ['qty_remaining', 'qty_produced','production_id']})        
        #close MO if WO is finished
        try:
            if workorder_status[0].get('qty_remaining') == 0:
                productionID = workorder_status[0].get('production_id')
                models.execute_kw(db, uid, password, 'mrp.production', 'button_mark_done',[productionID[0]])
        except (Exception):
            return -1
        return workorder_status[0].get('qty_produced') + workorder_status[0].get('qty_remaining')


    def writeRFIDTag(self, RFIDEPCTagInfo, RFIDTIDInfo):       
        try:
            workorder_status = models.execute_kw(db, uid, password, 'mrp.workorder', 'read',[workorder_id],{'fields': ['qty_remaining', 'qty_produced']})        
            if workorder_status[0].get('qty_remaining') == 0:
                return 2
        except (Exception):
            return -1
        try:
            serial_number_id = models.execute_kw(db, uid, password, 'mrp.workorder', 'search_read',[[['id','=',workorder_id]]], {'fields': ['x_serial_number_id']})
        except (Exception):
            return -2
        try:
            serial_number_id = models.execute_kw(db, uid, password, 'stock.production.lot', 'create',[{'name': RFIDEPCTagInfo, 'product_id': workorder_product_id[0], 'life_date': datetime.now() + timedelta(days=365), 'ref': RFIDTIDInfo}])
        except (Exception):
            return -3
        try:
            #Write serial number to Work Order
            models.execute_kw(db, uid, password, 'mrp.workorder', 'write',[[workorder_id],{'x_serial_number_id': serial_number_id}])
        except (Exception):
            return -4
        try:
            models.execute_kw(db, uid, password, 'mrp.workorder', 'update_serial_number',[[workorder_id]])
        except (Exception):
            return -5 
        return 1
    
    #compare to check if final lot ID has been updated, if update, show odoo update successful
    #def readFinalLotID(self):
    #    try:
    #        final_lot_id = models.execute_kw(db, uid, password, 'mrp.workorder', 'search_read', [[['id','=', workorder_id]]], {'fields': ['final_lot_id']})
    #        id = final_lot_id[0].get('final_lot_id')
    #    except (Exception):
    #        return ""
    #    return id[1]

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

    def startTimer(self):
        models.execute_kw(db, uid, password, 'mrp.workorder', 'button_start', [[workorder_id]])

    def pauseTimer(self):
        models.execute_kw(db, uid, password, 'mrp.workorder', 'button_pending',[[workorder_id]])

    #login is simulate the overall process, need to simplify
    def login(self, url, _db, username, _password): #db, username, password):
        #Log in Odoo
        try:
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

            global RFIDTagReady
            RFIDTagReady = True
        
            group_rec = models.execute_kw(db, uid, password, 'res.groups', 'search_read', [[['name','=','RFID Tag Creation']]], {'fields': ['users']})           
            list_of_user = group_rec[0].get('users')
            if uid in list_of_user:            
                return 1  #print('Has privilege')        
       
        except (Exception):
            return -1
       
        return 0 #print('Dont have privilege')  
