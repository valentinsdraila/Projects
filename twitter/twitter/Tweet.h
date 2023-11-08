#pragma once

#include <string>
#include <vector>
#include "Like.h"
#include <chrono>
class Tweet
{
public:
	enum class TweetType : uint8_t {
		eTweet,
		eRetweet
	};
	Tweet() = default;
	Tweet(const std::string& text,const time_t& time);
	~Tweet() = default;
	
	std::string GetText()const;
	int GetNrLikes()const;
	int GetNrCommentaries()const;
	int GetNrRetweets()const;
	time_t GetTime()const;

	void SetText(const std::string&);

	void DecreaseLike(const Like&);
	void AddLike(const Like&);
	void DecreaseCommentary();
	void AddRetweets();
	void DecreaseRetweets();

	bool operator==(const Tweet& p);

	int BinarySearch(const std::vector<Like>& likes, const Like& searchedLike);

protected:
	std::string m_text;
	time_t m_time;
	std::vector<Like> m_likes;  // TO DO: unordered map in loc de clasa like //(CHRONo) work with time  /time_t
	int retweets;
	TweetType m_tweetType;
};

