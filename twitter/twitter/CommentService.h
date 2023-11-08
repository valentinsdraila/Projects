#pragma once

#include <string>

#include "DatabaseConnection.h"

class CommentService
{
	void AddComment(const std::string& text, const std::string& date, const std::string& time, const std::string& id_user, int id_tweet);
	void DeleteComment(int id_comment);
	void EditComment(int id_comment, const std::string& newText);
	int GetId(const std::string& text, const std::string& date, const std::string& time, const std::string& id_user, int id_tweet);

	std::string GetText(int id_comment) const;
	std::string GetDate(int id_comment) const;
	std::string GetTime(int id_comment) const;
	std::string GetUser(int id_comment) const;
	int GetTweet(int id_comment) const;

	void SetText( int id_comment, const std::string& newText);
	void SetDate( int id_comment, const std::string& newDate);
	void SetTime( int id_comment, const std::string& newTime);
	void SetUser( int id_comment, const std::string& newUser);
	void SetTweet(int id_comment,  int newTweet);
};

