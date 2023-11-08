#pragma once

#include <string>

#include "Tweet.h"

class Comment:public Tweet
{
public:
	Comment() = default;
	Comment(Tweet* referenceTweet, const std::string& text, const time_t& time);
	~Comment() = default;

	void AddLike();
	void AddDislike();

private:
	Tweet *m_referenceTweet;
};

