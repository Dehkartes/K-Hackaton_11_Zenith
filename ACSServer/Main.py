from multiprocessing import Process, Pipe
import detect
import DBServer

if __name__ == "__main__":
	p1 = Process(target=detect.init_model_server)
	p1.start()
	DBServer.init_db_server()