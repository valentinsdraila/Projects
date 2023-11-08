#include "UserInterface.h"

void UserInterface::initWindow()
{
	this->m_window=new sf::RenderWindow(sf::VideoMode(1920,1080), "SFML works!");
	this->m_window->setFramerateLimit(120);
	this->m_window->setVerticalSyncEnabled(false);
}

void UserInterface::initStates()
{
	//(start)
	this->m_states.push(new LoginState(this->m_window,&this->m_states, socket));
	//(\start)
	
	//this->m_states.push(new FeedState(this->m_window,&this->m_states,socket));
	//this->m_states.push(new RegisterState(this->m_window,&this->m_states, mamaMia));
	//this->m_states.push(new TweetState(this->m_window, &this->m_states));
	//this->m_states.push(new ProfileState(this->m_window, &this->m_states));


}

UserInterface::UserInterface(TcpSocket& socket)
{
	this->socket = std::make_shared<TcpSocket>(socket);
	this->initWindow();

	this->initStates();
}

UserInterface::~UserInterface()
{
	delete this->m_window;
	while (!this->m_states.empty())
	{
		delete this->m_states.top();
		this->m_states.pop();
	}
}



void UserInterface::updateDT()
{
	this->m_dt = this->m_dtClock.restart().asSeconds();
}

void UserInterface::updateEvents()
{
	while (this->m_window->pollEvent(this->m_sfEvent))
	{
		if (this->m_sfEvent.type == sf::Event::Closed || sf::Keyboard::isKeyPressed(sf::Keyboard::Escape)) // conditie de esc
		{
			ClientFeatures::Network en = ClientFeatures::Exit;


			uint16_t aux = static_cast<uint16_t>(en);
			std::cout << aux << '\n';


			bool result = this->socket->Send((char*)&aux, sizeof(char));

			this->m_window->close();
		}
		//else if (m_sfEvent.type == m_sfEvent.MouseButtonReleased && m_sfEvent.mouseButton.button == sf::Mouse::Left)
		//	std::cout << "Left\n";
		else if (this->m_sfEvent.type == sf::Event::TextEntered)
		{
			if (this->m_states.top()->getCurrentState() == State::States::eLoginState)
			{
				LoginState* test = dynamic_cast<LoginState*> (m_states.top());
				test->SetText(m_sfEvent);
			}
			if (this->m_states.top()->getCurrentState() == State::States::eRegisterState)
			{
				RegisterState* test = dynamic_cast<RegisterState*> (m_states.top());
				test->SetText(m_sfEvent);
			}
			if (this->m_states.top()->getCurrentState() == State::States::eTweetState)
			{
				TweetState* test = dynamic_cast<TweetState*> (m_states.top());
				test->SetText(m_sfEvent);
			}
			if (this->m_states.top()->getCurrentState() == State::States::eFeedState)
			{
				FeedState* test = dynamic_cast<FeedState*> (m_states.top());
				test->SetText(m_sfEvent);
			}
			
		}	
		else if (this->m_sfEvent.type == this->m_sfEvent.MouseButtonReleased and this->m_sfEvent.mouseButton.button == sf::Mouse::Left)
			this->m_states.top()->btnReleased();
	}
}

void UserInterface::update()
{
	this->updateEvents();

	if (!this->m_states.empty())
	{
		this->m_states.top()->update(this->m_dt);

		if (this->m_states.top()->getQuit())
		{
			this->m_states.top()->endState();
			delete this->m_states.top();
			this->m_states.pop();
		}
	}
	else //end App
	{
		this->m_window->close();
	}
}

void UserInterface::render()
{
	this->m_window->clear();
	//render obj
	if (!this->m_states.empty())
	{
		this->m_states.top()->render(this->m_window);
	}

	this->m_window->display();
}

void UserInterface::run()
{
	while (this->m_window->isOpen())
	{
		this->updateDT();
		this->update();
		this->render();
	}
}
