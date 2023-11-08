#include "Logger.h"
#include <fstream>


Logger::Logger(std::ostream& os, Level minimumLevel) :
	m_Logfile(os), m_minimumLevel(minimumLevel)
{

}

void Logger::SetMinimumLogLevel(Level minimumLevel)
{
	this->m_minimumLevel = minimumLevel;
}