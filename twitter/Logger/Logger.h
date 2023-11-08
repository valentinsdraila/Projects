#pragma once
#include <iostream>
#include <string>

#ifdef LOGGER_EXPORTS
#define LOGGER_API _declspec(dllexport)
#else 
#define LOGGER_API _declspec(dllimport)
#endif //LOGGER_EXPORT

class LOGGER_API Logger
{
public:
	enum class Level
	{
		INFO,
		WARNING,
		ERROR
	};

public:
	Logger(std::ostream& os, Level minimumLevel = Level::INFO);

	template <typename T>
	void Log(const T& message, Level messageLevel);

	template <typename ... Args>
	void LocI(Level level, Args &&...param);

	void SetMinimumLogLevel(Level minimumLevel);

private:
	Level m_minimumLevel;
	std::ostream& m_Logfile;
};

template <typename T>
inline void Logger::Log(const T& message, Level level)
{
	if (level >= m_minimumLevel)
		m_Logfile << message << std::endl;
}

template <typename ... Args>
inline void Logger::LocI(Level level, Args &&...param)
{
	switch (level)
	{
	case Level::INFO:
	{
		m_Logfile << "[INFO]" << '\n';
	}
	}
	((m_Logfile << std::forward<Args>(param)), ...);
}