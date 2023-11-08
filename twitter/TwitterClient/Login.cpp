#include "Login.h"

Login::Login()
{
}

Login::Login(sf::RenderTarget* target)
{
	sf::Font arial;
	arial.loadFromFile("arial.ttf");
	this->m_font.loadFromFile("arial.ttf");
	Textbox loginTextBox(15, sf::Color::Black, true);
	loginTextBox.SetFont(this->m_font);
	loginTextBox.SetPosition({ 400, 250 });
	loginTextBox.SetLimit(1, 25);
	this->loginText = loginTextBox;
	std::string emptyString = "";
	float x = 400, y = 250, width = 300, height = 50;
	Button loginTextBoxButton(x, y, width, height, &this->m_font,emptyString, sf::Color::Blue, sf::Color::Blue, sf::Color::Blue);
	width = 100, y+=100;
	this->m_button.loginTextBox = loginTextBoxButton;
	Button login(x,y,width,height,&this->m_font, "Login", sf::Color::Blue, sf::Color::Blue, sf::Color::Blue);
	x += 150;
	this->m_button.login = login;
	Button _register(x, y, width, height, &this->m_font, "Register", sf::Color::Blue, sf::Color::Blue, sf::Color::Blue);
	this->m_button._register = _register;
}

void Login::DrawTo(sf::RenderWindow* target)
{
	sf::Event event;
	while (target->pollEvent(event))
	{
		switch (event.type)
		{
			case sf::Event::TextEntered:
			{
				loginText.Type(event);
				break;
			}
		}
	}
	this->loginText.DrawTo(target);
	this->m_button.loginTextBox.DrawTo(target);
	this->m_button.login.DrawTo(target);
	this->m_button._register.DrawTo(target);
}
