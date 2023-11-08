#include <SFML/Graphics.hpp>
#include "UI.h"
#include "UserInterface.h"
#include <iostream>
#include "../Network/TcpSocket.h"
#include <array>

/// <summary>
/// 
/// </summary>

/**

void callMethod(Network network)
{
	uint32_t nr = static_cast<uint32_t>(network);
	switch (nr)
	{
	case 0:
		///register
		std::cout << "Merge \n";
		break;
	case 1:
		///login
		std::cout << "Acum facem login\n";
		break;
	case 2:
		///MakePost

		break;
	case 3:
		///ShowFollowers

		break;
	case 4:
		///Like

		break;
	default:
		/// ...

		break;
	}
	

}
*/

int main(int argc, char* argv[])
{
	/*sf::RenderWindow window(sf::VideoMode(), "SFML works!", sf::Style::Fullscreen);
	window.clear();
	Design design(&window);
	Login login(&window);
	//sf::Font arial;
	//arial.loadFromFile("arial.ttf");
	//window.clear(sf::Color(204, 255, 255, 255));
	///*Textbox searchBar(15, sf::Color::White, true);
	//searchBar.SetFont(arial);
	//searchBar.SetPosition({ 400, 0 });
	//searchBar.SetLimit(1, 28);
	//*/
	///*Button homeButton("Home", {80,50}, 15, sf::Color::White, sf::Color::Black);
	//homeButton.SetPosition({ 100, 300 });
	//homeButton.SetFont(arial);*/
	//Textbox loginTextBox(15, sf::Color::Black, true);
	//loginTextBox.SetFont(arial);
	//loginTextBox.SetPosition({ 400, 250 });
	//loginTextBox.SetLimit(1, 25);
	//Button loginTextBoxButton("", { 300,50 }, 15, sf::Color::White, sf::Color::Black);
	//loginTextBoxButton.SetPosition({ 400,250 });
	//Button login("Login", { 150,50 }, 15, sf::Color(255, 102, 255, 255), sf::Color::Black);
	//login.SetPosition({ 400, 320 });
	//login.SetFont(arial);
	//Button _register("Register", { 150,50 }, 15, sf::Color(255, 0, 102, 255), sf::Color::Black);
	//_register.SetPosition({ 550, 320 });
	//_register.SetFont(arial);

	////--------
	//Design design(&window);

	////--------

	//while (window.isOpen())
	//{
	//	sf::Event event;

	//	/*if (sf::Keyboard::isKeyPressed(sf::Keyboard::Return))
	//	{
	//		searchBar.SetSelected(true);
	//	}
	//	else if (sf::Keyboard::isKeyPressed(sf::Keyboard::Escape))
	//	{
	//		searchBar.SetSelected(false);
	//	}
	//	while (window.pollEvent(event))
	//	{
	//		switch (event.type)
	//		{
	//		case sf::Event::Closed:
	//			window.close();
	//		case sf::Event::TextEntered:
	//			searchBar.Type(event);
	//			break;
	//		case sf::Event::MouseMoved:
	//			if (homeButton.IsMouseOver(window))
	//			{
	//				homeButton.SetBackColor(sf::Color(3, 252, 136, 120));
	//			}
	//			else
	//			{
	//				homeButton.SetBackColor(sf::Color::White);
	//			}
	//		}
	//	}
	//	*/
	//	
	//	while (window.pollEvent(event))
	//	{
	//		switch (event.type)
	//		{
	//		case sf::Event::MouseButtonPressed:
	//		{
	//			if (loginTextBoxButton.IsMouseOver(window))
	//				loginTextBox.SetSelected(true);
	//			else
	//				loginTextBox.SetSelected(false);
	//			break;
	//		}
	//		case sf::Event::TextEntered:
	//		{
	//			loginTextBox.Type(event);
	//			break;
	//		}
	//		case sf::Event::KeyPressed:
	//		{
	//			if (sf::Keyboard::isKeyPressed(sf::Keyboard::Escape))
	//			window.close();
	//			break;
	//		}
	//		

	//		case sf::Event::Closed:
	//			window.close();
	//		}
	//	}
	//	window.clear(sf::Color(204, 255, 255, 255));
	//	//searchBar.DrawTo(window);
	//	//homeButton.DrawTo(window);
	//	loginTextBoxButton.DrawTo(window);
	//	loginTextBox.DrawTo(window);
	//	login.DrawTo(window);
	//	_register.DrawTo(window);
	//	window.clear();
	//	design.DrawTo(window);
	//	window.display();
	//	
	//
/*
	while (window.isOpen())
	{
		sf::Event event;
		while (window.pollEvent(event))
		{
			if (event.type == sf::Event::Closed)
				window.close();
			if(event.type ==sf::Event::KeyPressed && sf::Keyboard::isKeyPressed(sf::Keyboard::Escape))
				window.close();

		}
		window.clear(sf::Color(255, 255, 255, 255));
		login.DrawTo(&window);
		if(false)
			design.DrawTo(&window);
		window.display();
	}
	*/

	//UI ui;  //reinclude lib SFML\Graphics.hpp
	//ui.Display();
    //Network network = Network::Register;
	//callMethod(network);
	//callMethod(Network::Login);
//MakeConnection(argc, argv);
	std::string username = "Test";
	//std::thread t1{ MakeConnection , argc, argv};
	//auto now = std::chrono::system_clock::now();
	//std::this_thread::sleep_until(now + std::chrono::seconds(5));
	//t1.join();
	TcpSocket socket;
	socket.Connect("192.168.2.114", 27015);
	UserInterface UI(socket);
	UI.run();
	return 0;
}

