#pragma once
#include "State.h"
#include "FeedState.h"
#include "RegisterState.h"

#include <array>

class LoginState : public State
{
private:
	struct TwitterLoginButtons {
		Button* Login;
		Button* Register;
		Button* TextBoxButton;
	};
	TwitterLoginButtons m_button;
	
	Textbox* m_textbox;
	sf::RectangleShape m_enterBox;
	sf::Event m_sfEvent;

	sf::Sprite m_twitterLogo;
	sf::Texture m_twitterImg;

	
	int m_count;

	std::shared_ptr<TcpSocket> mamaMia;

public:

	LoginState(sf::RenderWindow* window, std::stack<State*> *states, std::shared_ptr<TcpSocket> socket);
	virtual ~LoginState();

	void endState();
	void updateKeybinds(const float& dt);
	void updateButtons();
	void updateUseButtons();
	void update(const float& dt);
	void SetText(sf::Event);

	void renderButtons(sf::RenderTarget* target);
	void render(sf::RenderTarget* target = NULL);

	void initButtons(sf::RenderTarget* target);
	


};
