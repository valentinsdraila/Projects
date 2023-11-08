#pragma once

#include <string>
#include <vector>
#include <iostream>

#include "PriorityQueue.h"
#include "UserService.h"


class User
{
public:
	User() = default;
	User(const std::string&,const std::string&,const std::string&,const std::string&,const std::string&,const std::string&);
	User(const std::string&);
	User(const std::string&, bool);
	~User() = default;
	User(const User&);

	std::string GetUsername()const;
	std::string GetName()const;
	std::string GetBirthday()const;
	std::string GetBio()const;
	std::string GetWebsite()const;
	std::string GetLocation()const;

	void SetUsername(const std::string& username);
	void SetName(const std::string& name);
	void SetBio(const std::string& bio);
	void SetWebsite(const std::string& website);
	void SetBirthday(const std::string& birthday);
	void SetLocation(const std::string& location);

	void CreateFeedPosts();

	std::string GetPriorityPost();

	bool operator ==(const User& user);
private:
	std::string	m_username;
	std::string m_name;
	std::string m_bio;
	std::string m_website;
	std::string m_birthday;
	std::string m_location;

	PriorityQueue <std::string> m_posts;
};

