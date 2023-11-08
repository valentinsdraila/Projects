
#pragma once

#include "State.h"
#include "FeedState.h"
#include "LoginState.h"
#include "RegisterState.h"
#include "TweetState.h"
#include "ProfileState.h"
#include "../Network/TcpSocket.h"
#include "ClientFeatures.h"

class UserInterface
{
private:
	sf::RenderWindow* m_window;
	sf::Event m_sfEvent;

	sf::Clock m_dtClock;
	float m_dt;
	

	std::stack<State*> m_states;

	void initWindow();
	void initStates();
	
	std::shared_ptr<TcpSocket> socket;
public:
	UserInterface(TcpSocket& socket);
	virtual ~UserInterface();

	void updateDT();
	void updateEvents();
	void update();
	void render();
	void run();
};

