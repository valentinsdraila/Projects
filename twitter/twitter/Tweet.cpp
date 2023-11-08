#include "Tweet.h"

Tweet::Tweet(const std::string& text,const time_t& time):
	m_text(text),
	m_time(time)
{
}

std::string Tweet::GetText() const
{
	return m_text;
}

int Tweet::GetNrLikes() const
{
	return m_likes.size();
}

int Tweet::GetNrCommentaries() const
{
	return 0;
}

int Tweet::GetNrRetweets() const
{
	return retweets;
}
time_t Tweet::GetTime() const
{
	return m_time;
}

void Tweet::SetText(const std::string& newText)
{
	m_text.erase();
	m_text.append(newText);
}

void Tweet::DecreaseLike(const Like& oldLike)
{
	int position=BinarySearch(m_likes, oldLike);
	m_likes.erase(m_likes.begin() + position);
}

void Tweet::AddLike(const Like& newLike)
{
	m_likes.push_back(newLike);
}

void Tweet::DecreaseCommentary()
{
}

void Tweet::AddRetweets()
{
}

void Tweet::DecreaseRetweets()
{
}

bool Tweet::operator==(const Tweet& tweet)
{
	if (this->m_text != tweet.GetText())
		return false;
	if (this->m_time != tweet.GetTime())
		return false;
	return false;
}

int Tweet::BinarySearch(const std::vector<Like>& likes,const Like& searchedLike)
{
	int left = 0, right = likes.size(), position = -1;
	while (left < right && position == -1)
	{
		int mijloc = (left + right) / 2;
		if (likes[mijloc] == searchedLike)
		{
			position = mijloc;
		}
		else
		{
			if (likes[mijloc] < searchedLike)
			{
				left = ++mijloc;
			}
			else
			{
				right = --mijloc;
			}
		}
	}
	if (likes[left] == searchedLike)
	{
		position = left;
	}
	return position;
}