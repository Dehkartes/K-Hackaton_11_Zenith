# This is a sample Python script.
import pymysql
import pymysql.cursors
from flask import Flask, jsonify

def contect_db():
    conn = pymysql.connect(host='localhost', user='admin',
                           password='1592', db='k-hackaton_11',
                           charset='utf8', cursorclass=pymysql.cursors.DictCursor)

    cursor = conn.cursor()
    sql = "SELECT * FROM crackdown"

    cursor.execute(sql)
    rows = cursor.fetchall()
    # rows = [list(rows[x]) for x in range(len(rows))]
    # d = json.dumps(rows, default=str)
    conn.commit()
    conn.close()
    return rows

app = Flask(__name__)

@app.route('/database', methods=['GET'])
def database():
    dbData = contect_db()
    return jsonify(dbData)  # 받아온 데이터를 다시 전송

def init_db_server():
	app.run(host='0.0.0.0', port=5000, debug=True)