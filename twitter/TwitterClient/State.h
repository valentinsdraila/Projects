#pragma once

#include<iostream>
#include<ctime>
#include<cstdlib>
#include<vector>
#include<stack>
#include<map>
#include <array>
#include "../Network/TcpSocket.h"

#include<SFML\Graphics.hpp>
#include<SFML\Window.hpp>
#include<SFML\System.hpp>


#include "ClientFeatures.h"
#include "Button.h"
#include "Textbox.h"
#include "Tweetbox.h"


class State
{
private:
	
protected:

	struct TwitterColor {
		sf::Color Blue; //twitter logo,tweet button
		sf::Color Black;//background
		sf::Color DarkGray;//left stuff and button highlight
		sf::Color LightGray;//dunno
		sf::Color ExtraLightGray;// follow buttons and stuff and text??
		sf::Color ExtraExtraLightGray;//current button text highlight
	};
	TwitterColor m_color;
	
	//original stack in UserInterface
	std::stack<State*> *m_states;

	sf::RenderWindow* m_window;
	bool quit;
	int m_buttonRelease;

	sf::Font m_font;
	
	sf::Vector2f m_mousePosView;

public:

	enum States : uint16_t
	{
		eLoginState,
		eRegisterState,
		eFeedState,
		eTweetState,
		eProfileState
	};

	enum UserImg : uint8_t
	{
		eAnon1,
		eAnon2,
		eFem1,
		eFem2,
		eFem3,
		eMale1,
		eMale2
	};
	enum IconImg : uint8_t
	{
		eTwitter,
		eLike,
		eRetweet,
		eComment
	};

	States getCurrentState()const;

	State(sf::RenderWindow* window, std::stack<State*> *states);
	virtual ~State();

	const bool& getQuit() const;
	virtual void checkforQuit();
	virtual void endState()=0;

	void btnReleased();
	//updates
	virtual void updateMousePos();
	virtual void updateKeybinds(const float& dt) =0;
	virtual void update(const float& dt) = 0;
	//

	void initColor();
	void initFont();

	virtual void render(sf::RenderTarget * target=NULL)=0;

	void SetNetworkEnum(const ClientFeatures::Network& network);

protected:
	ClientFeatures::Network m_network;
	
	States m_enumStates;
};


