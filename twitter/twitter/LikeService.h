#pragma once

#include <string>

#include "Tweet.h"
#include "User.h"

class LikeService
{
public:
	void AddLike(std::string username,int id_tweet);
	void DeleteLike(int id_like);
	int GetId(std::string id_user, int id_tweet);
};

