#pragma once
#include <iostream>
#include <SFML/Graphics.hpp>
#include "Button.h"
#include "Textbox.h"
class Login
{
public:
	Login();
	Login(sf::RenderTarget* target);
	void DrawTo(sf::RenderWindow* target);
private:
	sf::Font m_font;
	struct TwitterLoginButton
	{
		Button loginTextBox;
		Button login;
		Button _register;
	};
	Textbox loginText;
	TwitterLoginButton m_button;
};

