#pragma once

#include "Tweet.h"

class Retweet:public Tweet
{
public:
	Retweet() = default;
	Retweet(Tweet* referenceTweet, const std::string& text, const time_t& time);
	~Retweet();

	Tweet* GetReferenceTweet();

private:
	Tweet* m_referenceTweet;
};

