#pragma once
#include <SFML/Graphics.hpp>
#include <iostream>
#include "../Network/TcpSocket.h"
#include <array>
#include <sstream>
#include "libpq-fe.h"
#include "DatabaseConnection.h"
#include "UserService.h"
#include "../Logger/Logger.h"
#include <thread>
#include <ctime>
#include <iomanip>
#include "Features.h"
#include "Tweet.h"
#include "User.h"
#include "FriendsService.h"
#include "TweetService.h"
#include <unordered_set>
#include "PriorityQueue.h"
#include <queue>
#pragma warning(disable:4996)
#ifdef _MSC_VER
#define _CRT_SECURE_NO_WARNINGS
#endif
class Twitter
{
public:
	Twitter();
	~Twitter() = default;

	std::vector<Tweet> GetTweets()const;
	std::unordered_set<std::string> GetUsers()const;

	void SetTweet(const Tweet& tweet);
	void MakePostMethod();
	void LoginMethod();
	void RegisterMethod();
	void AddFriendMethod();

	std::string RecieveString()const;


	void MakePriorityQueue();
	void MakeFeedPriorityQueue();

	void operator()(TcpSocket& client, Logger& log);
	
	void ShowNextPost();
	void ShowPreviousPost();

	void ProfileShowNextPost();
	void ProfilShowPreviousPost();

private:
	std::vector<Tweet> m_tweets; 
	std::unordered_set<std::string> m_users;
	
	Logger* log;

	std::string m_currentUser;
	bool m_isConnected;
	std::shared_ptr<TcpSocket> m_client;


	std::priority_queue<std::string, std::vector<std::string>, std::greater<std::string>> m_oldPosts;
	PriorityQueue<std::string> m_priorityQueue;

	PriorityQueue<std::string> m_profilePriorityQueue;
	std::priority_queue<std::string, std::vector<std::string>, std::greater<std::string>> m_profileOldPosts;
};

