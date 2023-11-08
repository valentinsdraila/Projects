#include "ProfileState.h"

ProfileState::ProfileState(sf::RenderWindow* window, std::stack<State*>* states, std::shared_ptr<TcpSocket> socket)
	:State(window,states)
{
	mamaMia = socket;
	initBox(window);
	initButtons(window);
	initLines(window);
	initTweetbox(window);
	m_enumStates=States::eProfileState;
	
	std::array<char, 512> rBuffer;
	int revieved;
	std::string recieved;
	mamaMia->Receive(rBuffer.data(), rBuffer.size(), revieved);

	std::copy(rBuffer.begin(), rBuffer.begin() + revieved, std::back_inserter(recieved));

	this->m_tweet->SetText(recieved);

}

ProfileState::~ProfileState()
{
	delete this->m_button.Back;
	delete this->m_button.NextTweet;
	delete this->m_button.PrevTweet;
}

void ProfileState::endState()
{
	std::cout << "Ending ProfileState\n";
}

void ProfileState::updateKeybinds(const float& dt)
{
	this->checkforQuit();
}

void ProfileState::updateButtons()
{
	this->m_button.Back->update(m_mousePosView);
	this->m_button.NextTweet->update(m_mousePosView);
	this->m_button.PrevTweet->update(m_mousePosView);
}

void ProfileState::updateUseButtons()
{
	if (this->m_button.Back->getButtonState() == 2 and m_buttonRelease == 1)
	{

		this->m_states->pop();
		std::cout << "backProfile";
		m_buttonRelease = 0;

	}
	if (this->m_button.NextTweet->getButtonState() == 2 and m_buttonRelease == 1)
	{
		std::string username;
		ClientFeatures::Network en = ClientFeatures::ProfileNextPost;

		uint16_t aux = static_cast<uint16_t>(en);
		std::cout << aux << '\n';


		bool result = mamaMia->Send((char*)&aux, sizeof(char));
		if (result)
		{
			std::array<char, 512> rBuffer;
			int revieved;
			std::string recieved;
			mamaMia->Receive(rBuffer.data(), rBuffer.size(), revieved);

			std::copy(rBuffer.begin(), rBuffer.begin() + revieved, std::back_inserter(recieved));

			this->m_tweet->SetText(recieved);
		}
		m_buttonRelease = 0;
	}
	if (this->m_button.PrevTweet->getButtonState() == 2 and m_buttonRelease == 1)
	{
		std::string username;
		ClientFeatures::Network en = ClientFeatures::ProfilePreviousPost;

		uint16_t aux = static_cast<uint16_t>(en);
		std::cout << aux << '\n';


		bool result = mamaMia->Send((char*)&aux, sizeof(char));
		if (result)
		{
			std::array<char, 512> rBuffer;
			int revieved;
			std::string recieved;
			mamaMia->Receive(rBuffer.data(), rBuffer.size(), revieved);

			std::copy(rBuffer.begin(), rBuffer.begin() + revieved, std::back_inserter(recieved));

			this->m_tweet->SetText(recieved);
		}
		m_buttonRelease = 0;
	}
}

void ProfileState::update(const float& dt)
{
	this->updateButtons();
	this->updateUseButtons();
	this->m_tweet->updateTweetbox(m_mousePosView);
	this->updateMousePos();
	this->updateKeybinds(dt);
}

void ProfileState::renderButtons(sf::RenderTarget* target)
{
	this->m_button.Back->render(target);
	this->m_button.NextTweet->render(target);
	this->m_button.PrevTweet->render(target);
}

void ProfileState::renderBox(sf::RenderTarget* target)
{
	target->draw(m_profileBox);
}

void ProfileState::renderLines(sf::RenderTarget* target)
{
	target->draw(m_nameLine);
	target->draw(m_bioLine);
	
}

void ProfileState::renderTweetbox(sf::RenderTarget* target)
{
	m_tweet->render(target);
}

void ProfileState::render(sf::RenderTarget* target)
{
	if (!target)
		target = this->m_window;

	renderBox(target);
	renderLines(target);
	renderButtons(target);
	renderTweetbox(target);
}


void ProfileState::initButtons(sf::RenderTarget* target)
{
	this->m_button.Back = new Button(target->getSize().x / 6, 25,
		target->getSize().x / 8, target->getSize().y / 20,
		&this->m_font, "Back",
		this->m_color.Black, this->m_color.DarkGray,
		this->m_color.Blue, this->m_color.DarkGray);

	this->m_button.NextTweet = new Button(target->getSize().x / 12*8, target->getSize().y/12*10,
		target->getSize().x / 12, target->getSize().y / 20,
		&this->m_font, "Next",
		this->m_color.Blue, this->m_color.LightGray,
		this->m_color.Blue, sf::Color::Black);
	this->m_button.PrevTweet = new Button(target->getSize().x / 4, target->getSize().y / 12 * 10,
		target->getSize().x / 12, target->getSize().y / 20,
		&this->m_font, "Previous",
		this->m_color.Blue, this->m_color.LightGray,
		this->m_color.Blue, sf::Color::Black);
}

void ProfileState::initBox(sf::RenderWindow* target)
{
	this->m_profileBox = sf::RectangleShape(sf::Vector2f(target->getSize().x / 3*2, target->getSize().y -50));
	this->m_profileBox.setPosition(sf::Vector2f(target->getSize().x / 6, 25));
	this->m_profileBox.setFillColor(this->m_color.Black);
	this->m_profileBox.setOutlineColor(this->m_color.DarkGray);
	this->m_profileBox.setOutlineThickness(1);
}

void ProfileState::initLines(sf::RenderWindow* target)
{
	
	this->m_nameLine = sf::VertexArray(sf::Lines, 2);
	this->m_nameLine[0] = sf::Vector2f(target->getSize().x /6, target->getSize().y / 12+ target->getSize().y/20+20);
	this->m_nameLine[1] = sf::Vector2f(target->getSize().x /6*5, target->getSize().y / 12 + target->getSize().y / 20 + 20);
	this->m_nameLine[0].color = this->m_color.DarkGray;
	this->m_nameLine[1].color = this->m_color.DarkGray;
	this->m_bioLine = sf::VertexArray(sf::Lines, 2);
	this->m_bioLine[0] = sf::Vector2f(target->getSize().x / 6, target->getSize().y / 12*5-20);
	this->m_bioLine[1] = sf::Vector2f(target->getSize().x / 6*5, target->getSize().y / 12*5-20);
	this->m_bioLine[0].color = this->m_color.DarkGray;
	this->m_bioLine[1].color = this->m_color.DarkGray;
	
}

void ProfileState::initTweetbox(sf::RenderTarget* target)
{
	this->m_tweet = new Tweetbox(target->getSize().x / 12*2+20, target->getSize().y / 12*5,
		sf::Vector2f(target->getSize().x / 12*8-40, target->getSize().y / 12*5-20), 50, 100,
		&this->m_font, sf::Color::Black,
		"Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat mas");
	this->m_tweet->initButtons();
}
