#pragma warning(disable:4996)
#ifdef _MSC_VER
#define _CRT_SECURE_NO_WARNINGS
#endif

#include "Twitter.h"
#include <fstream>

int main()
{
	DatabaseConnection db;
	db.CreateTables(db.GetConn());
	TcpSocket listener;
	listener.Listen(27015);
	while (true)
	{

		/// cleint conected on socket : client
		TcpSocket client = listener.Accept();
		Logger Log(std::cout, Logger::Level::INFO);
		Log.Log("Client connected!", Logger::Level::INFO);
		//Create thread 
		std::thread t1( Twitter(), std::ref(client), std::ref(Log) );
		t1.join();
		//DoSomething(client);
		
	}
	return 0;
}