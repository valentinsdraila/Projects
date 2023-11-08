#include "UI.h"

void UI::Display()
{
	sf::RenderWindow window(sf::VideoMode(), "SFML works!", sf::Style::Fullscreen);
	/*Textbox searchBar(15, sf::Color::White, true);
	searchBar.SetFont(arial);
	searchBar.SetPosition({ 400, 0 });
	searchBar.SetLimit(1, 28);
	*/
	/*Button homeButton("Home", {80,50}, 15, sf::Color::White, sf::Color::Black);
	homeButton.SetPosition({ 100, 300 });
	homeButton.SetFont(arial);*/
	Design design(&window);
	sf::Font arial;
	arial.loadFromFile("arial.ttf");
	Textbox loginTextBox(15, sf::Color::Black, true);
	loginTextBox.SetPosition({ 400, 250 });
	loginTextBox.SetLimit(1, 25);
	loginTextBox.SetFont(arial);
	std::string emptyString = "";
	float x = 400, y = 250, width = 300, height = 50;
	Button loginTextBoxButton(x, y, width, height, &arial, emptyString, sf::Color::Blue, sf::Color::Black, sf::Color::Blue);
	width = 100, y += 100;
	Button login(x, y, width, height, &arial, "Login", sf::Color::Blue, sf::Color::Black, sf::Color::Blue);
	x += 150;
	Button _register(x, y, width, height, &arial, "Register", sf::Color::Blue, sf::Color::Black, sf::Color::Blue);


	bool OK = false;
	while (window.isOpen())
	{
		sf::Event event;
		updateMousePositions(&window);
		design.m_button.LogoButton.update(mousePosView);
		design.m_button.Home.update(mousePosView);
		design.m_button.Explore.update(mousePosView);
		design.m_button.Notifications.update(mousePosView);
		design.m_button.Messages.update(mousePosView);
		design.m_button.Lists.update(mousePosView);
		design.m_button.Bookmarks.update(mousePosView);
		design.m_button.Profile.update(mousePosView);
		design.m_button.More.update(mousePosView);
		design.m_button.Tweet.update(mousePosView);
		int buttonLogoID = 1;
		login.update(mousePosView);
		_register.update(mousePosView);
		doButtonFunctions(buttonLogoID, &window);
		
		

		/*if (sf::Keyboard::isKeyPressed(sf::Keyboard::Return))
		{
			searchBar.SetSelected(true);
		}
		else if (sf::Keyboard::isKeyPressed(sf::Keyboard::Escape))
		{
			searchBar.SetSelected(false);
		}
		while (window.pollEvent(event))
		{
			switch (event.type)
			{
			case sf::Event::Closed:
				window.close();
			case sf::Event::TextEntered:
				searchBar.Type(event);
				break;
			case sf::Event::MouseMoved:
				if (homeButton.IsMouseOver(window))
				{
					homeButton.SetBackColor(sf::Color(3, 252, 136, 120));
				}
				else
				{
					homeButton.SetBackColor(sf::Color::White);
				}
			}
		}
		*/

		while (window.pollEvent(event))
		{
			switch (event.type)
			{
				/*case sf::Event::MouseButtonPressed:
				{
					if (loginTextBoxButton.IsMouseOver(window))
						loginTextBox.SetSelected(true);
					else
						loginTextBox.SetSelected(false);
					break;
				}
				*/
			case sf::Event::TextEntered:
			{
				loginTextBox.Type(event);
				break;
			}
			case sf::Event::KeyPressed:
			{
				if (sf::Keyboard::isKeyPressed(sf::Keyboard::Escape))
					window.close();
				break;
			}
			/*case sf::Event::MouseButtonPressed:
			{
					if (loginTextBoxButton.isPressed())
						loginTextBox.SetSelected(true);
					else
					{
						loginTextBox.SetSelected(false);
						std::cout << "Nu";
					}
					break;
			}*/
			case sf::Event::Closed:
				window.close();
			}
		}
		window.clear(sf::Color(224, 255, 255, 255));
		if (login.isPressed() /*&& username is valid*/)
			OK = 1;
		else
			if (_register.isPressed() /*&&username is valid*/)
				OK = 1;
		if (OK==false)
		{
			/*
			loginTextBoxButton.DrawTo(&window);
			loginTextBox.DrawTo(&window);
			login.DrawTo(&window);
			_register.DrawTo(&window);
			*/
		}
		else
		design.DrawTo(&window);
		window.display();


	}
}

void UI::updateMousePositions(sf::RenderWindow* window)
{
	this->mousePosScreen = sf::Mouse::getPosition();
	this->mousePosWindow = sf::Mouse::getPosition(*window);
	this->mousePosView = window->mapPixelToCoords(sf::Mouse::getPosition(*window));
	
}


//poate fiecare buton are un id?

void UI::doButtonFunctions(int buttonID, sf::RenderWindow* window)
{

	switch (buttonID)
	{
	case 1:

		break;
	case 2:

		break;

	}
	
}


UI::UI()
	{

	}


	
