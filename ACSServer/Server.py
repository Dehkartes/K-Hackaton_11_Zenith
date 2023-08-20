import socket # 소켓 프로그래밍에 필요한 API를 제공하는 모듈
import struct # 바이트(bytes) 형식의 데이터 처리 모듈
import pickle # 객체의 직렬화 및 역직렬화 지원 모듈
import cv2 # OpenCV(실시간 이미지 프로세싱) 모듈
import pymysql
from datetime import datetime

class Server:
	conn = None
	cur = None
	def __init__(self) -> None:
		self.frame = None
		Server.conn = pymysql.connect(host='127.0.0.1', user='Admin', password='1592', db='k-hackaton_11', charset='utf8')
		Server.cur = Server.conn.cursor()
		# 서버 ip 주소 및 port 번호
		ip = '192.168.0.26'
		port = 60001

		# 소켓 객체 생성
		server_socket = socket.socket(socket.AF_INET,socket.SOCK_STREAM)

		# 소켓 주소 정보 할당
		server_socket.bind((ip, port))
		print(server_socket)
		# 연결 리스닝(동시 접속) 수 설정
		server_socket.listen(10) 

		print('클라이언트 연결 대기')

		# 연결 수락(클라이언트 (소켓, 주소 정보) 반환)
		self.client_socket, self.address = server_socket.accept()
		print('클라이언트 ip 주소 :', self.address[0])

		# 수신한 데이터를 넣을 버퍼(바이트 객체)
		self.data_buffer = b""

		# calcsize : 데이터의 크기(byte)
		# - L : 부호없는 긴 정수(unsigned long) 4 bytes
		self.data_size = struct.calcsize("L")

	def remain(self):
		while True:
			print("test")

	def readframe(self):
		# 설정한 데이터의 크기보다 버퍼에 저장된 데이터의 크기가 작은 경우
		while len(self.data_buffer) < self.data_size:
			# 데이터 수신
			self.data_buffer += self.client_socket.recv(4096)

		# 버퍼의 저장된 데이터 분할
		packed_data_size = self.data_buffer[:self.data_size]
		self.data_buffer = self.data_buffer[self.data_size:] 
		
		# struct.unpack : 변환된 바이트 객체를 원래의 데이터로 반환
		# - > : 빅 엔디안(big endian)
		#   - 엔디안(endian) : 컴퓨터의 메모리와 같은 1차원의 공간에 여러 개의 연속된 대상을 배열하는 방법
		#   - 빅 엔디안(big endian) : 최상위 바이트부터 차례대로 저장
		# - L : 부호없는 긴 정수(unsigned long) 4 bytes 
		frame_size = struct.unpack(">L", packed_data_size)[0]
		
		# 프레임 데이터의 크기보다 버퍼에 저장된 데이터의 크기가 작은 경우
		while len(self.data_buffer) < frame_size:
			# 데이터 수신
			self.data_buffer += self.client_socket.recv(4096)
		
		# 프레임 데이터 분할
		frame_data = self.data_buffer[:frame_size]
		self.data_buffer = self.data_buffer[frame_size:]
		
		#print("수신 프레임 크기 : {} bytes".format(frame_size))
		
		# loads : 직렬화된 데이터를 역직렬화
		# - 역직렬화(de-serialization) : 직렬화된 파일이나 바이트 객체를 원래의 데이터로 복원하는 것
		frame = pickle.loads(frame_data)
		
		# imdecode : 이미지(프레임) 디코딩
		# 1) 인코딩된 이미지 배열
		# 2) 이미지 파일을 읽을 때의 옵션
		#    - IMREAD_COLOR : 이미지를 COLOR로 읽음
		frame = cv2.imdecode(frame, cv2.IMREAD_COLOR)
		# #프레임 출력
		# cv2.imshow('Frame', frame)
		
		# # 'q' 키를 입력하면 종료
		# key = cv2.waitKey(1) & 0xFF
		# if key == ord("q"):
		# 	break
		return frame
		
	def create_stream(self):
		while True:
			yield self.readframe()

	
	def stop(self):
		Server.conn.close()
		# 소켓 닫기
		self.client_socket.close()
		self.server_socket.close()
		print('연결 종료')

		# 모든 창 닫기
		cv2.destroyAllWindows()

	def Record(self, CCTV_ID: int, Crackdown_Type: int, Locatioin_Id: int):
		Server.cur.execute(f'insert into crackdown(CCTV_ID, Location_ID, Date, Crackdown_Type) values(\'{CCTV_ID}\', \'{Locatioin_Id}\', \'{datetime.now()}\', \'{Crackdown_Type}\')')
		Server.conn.commit()

if __name__ == '__main__':
	sv = Server()
	for i in sv.create_stream():
		cv2.imshow('f', i)
		key = cv2.waitKey(1) & 0xFF
		if key == ord("q"):
			break