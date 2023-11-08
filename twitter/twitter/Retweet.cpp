#include "Retweet.h"

Retweet::Retweet(Tweet* referenceTweet, const std::string& text, const time_t& time):
	Tweet(text,time),
	m_referenceTweet(referenceTweet)
{
}

Retweet::~Retweet()
{
	//*****************************************
	//*********							*******
	//*********		~Tweet();	???		*******
	//*********							*******
	//*****************************************

	delete m_referenceTweet;
}

Tweet* Retweet::GetReferenceTweet()
{
	return m_referenceTweet;
}
