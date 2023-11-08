#pragma once

#include <string>

#include "DatabaseConnection.h"

class RetweetService
{
	void AddRetweet(const std::string& text, const std::string& date, const std::string& time, const std::string& id_user,int id_tweet);
	void DeleteRetweet(int id_retweet);
	void EditRetweet(int id_retweet, const std::string& newText);
	int GetId(const std::string& text, const std::string& date, const std::string& time, const std::string& id_user, int id_tweet);

	std::string GetText(int id_retweet) const;
	std::string GetDate(int id_retweet) const;
	std::string GetTime(int id_retweet) const;
	std::string GetUser(int id_retweet) const;
	int GetTweet(int id_retweet) const;

	void SetText( int id_retweet, const std::string& newText);
	void SetDate(int id_retweet, const std::string& newDate);
	void SetTime(int id_retweet, const std::string& newTime);
	void SetUser(int id_retweet, const std::string& newUser);
	void SetTweet(int id_retweet, int newTweet);
};

