#pragma once

#include "DatabaseConnection.h"
#include <vector>
#include <string>

class TweetService
{
public:
	void AddTweet(const std::string& text, const std::string& date, const std::string& time, const std::string& id_user);
	void DeleteTweet(int id_tweet);
	void EditTweet(int id_tweet, const std::string& newText);
	int GetId(const std::string& text, const std::string& date, const std::string& time, const std::string& id_user);

	std::string GetUserByTime(const std::string& time);

	std::string GetTweet(const std::string& time);

	std::vector<std::string> GetDate(const std::string& username);
	std::vector<std::string> GetTime(const std::string& username);
};

