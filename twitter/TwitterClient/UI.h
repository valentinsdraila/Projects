#pragma once
#include "Textbox.h"
#include "Button.h"
#include "Design.h"
class UI
{
private:
	sf::Vector2i mousePosScreen;
	sf::Vector2i mousePosWindow;
	sf::Vector2f mousePosView;

public:
	UI();
	void Display();

	
	void updateMousePositions(sf::RenderWindow* window);
	void doButtonFunctions(int buttonID,sf::RenderWindow* window);
	
};

