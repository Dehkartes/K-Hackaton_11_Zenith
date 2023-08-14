# This is a sample Python script.
import json
import pymysql
import pymysql.cursors
from flask import Flask, jsonify

def contect_db():
    conn = pymysql.connect(host='192.168.55.78', user='user01',
                           password='user01', db='db01',
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


if __name__ == "__main__":
    app.run(host='0.0.0.0', port=5000, debug=True)