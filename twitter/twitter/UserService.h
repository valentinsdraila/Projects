#pragma once

#include <string>
#include <iostream>
#include <vector>

#include "DatabaseConnection.h"

class UserService
{
public:
	void AddUser(const std::string& username,const std::string& birthday,const std::string& name,const std::string& bio,const std::string& website,const std::string& location);
	void RemoveUser(const std::string& username);
	bool CheckUser(const std::string& username);

	void SetBio(const std::string& username, const std::string& bio);
	void SetWebsite(const std::string& username, const std::string& website);
	void SetLocation(const std::string& username, const std::string& location);

	std::string GetBirthday(const std::string& username)const;
	std::string GetName(const std::string& username)const;
	std::string GetBio(const std::string& username)const;
	std::string GetWebsite(const std::string& username)const;
	std::string GetLocation(const std::string& username)const;
	std::vector<std::string> GetUser(const std::string& username)const;
};

