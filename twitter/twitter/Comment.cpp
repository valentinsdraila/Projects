#include "Comment.h"

Comment::Comment(Tweet* referenceTweet, const std::string& text, const time_t& time) :
	Tweet(text, time),
	m_referenceTweet(referenceTweet)
{
}


void Comment::AddLike()
{
}

void Comment::AddDislike()
{
}
