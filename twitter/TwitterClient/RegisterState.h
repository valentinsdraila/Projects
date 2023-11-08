#pragma once
#include "State.h"
#include "FeedState.h"
class RegisterState : public State
{
private:
	struct TwitterRegisterButtons {
		Button* Back;
		Button* CreateAccount;
		Button* Username;
		Button* Bio;
		Button* Website;
		Button* Birthday;
		Button* Name;
		Button* Location;
	};

	struct TwitterRegisterTextBoxes {
		Textbox* Username;
		Textbox* Bio;
		Textbox* Website;
		Textbox* Birthday;
		Textbox* Name;
		Textbox* Location;
	};

	struct TwitterRegisterLabels {
		Button* Username;
		Button* Bio;
		Button* Website;
		Button* Birthday;
		Button* Name;
		Button* Location;
	};

	TwitterRegisterButtons m_button;

	TwitterRegisterTextBoxes m_textBox;

	TwitterRegisterLabels m_labels;

	sf::RectangleShape m_registerBox;
	
	Textbox* m_registerPointer;

	std::shared_ptr<TcpSocket> mamaMia;
public:
	RegisterState(sf::RenderWindow* window, std::stack<State*> *states, std::shared_ptr<TcpSocket> socket);
	virtual ~RegisterState();


	void endState();
	void updateKeybinds(const float& dt);
	void updateButtons();
	void updateUseButtons();
	void update(const float& dt);

	void SetText(const sf::Event&);

	void renderButtons(sf::RenderTarget* target);
	void renderBox(sf::RenderTarget* target);
	void render(sf::RenderTarget* target = NULL);

	void initButtons(sf::RenderTarget* target);
	void initBox(sf::RenderTarget* target);

};
