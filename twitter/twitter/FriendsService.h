#pragma once

#include <string>
#include <vector>

#include "DatabaseConnection.h"

class FriendsService
{
public:
	void AddFriend(const std::string& username, const std::string& friendUsername);
	void RemoveFriend(const std::string& username, const std::string& friendUsername);

	bool VerifyFriends(const std::string& username, const std::string& friendUsername);

	std::vector<std::string> GetFriendsList(const std::string& username);
};

