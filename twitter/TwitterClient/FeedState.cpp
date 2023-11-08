#include "FeedState.h"

FeedState::FeedState(sf::RenderWindow* window, std::stack<State*> *states, std::shared_ptr<TcpSocket> socket)
	: State(window,states)
{
	mamaMia = socket;
	initLines(window);
	initButtons(window);
	initTweetbox(window); 
	initSpritesTextures(window);
	m_enumStates = States::eFeedState;

	std::array<char, 512> rBuffer;
	int revieved;
	std::string recieved;
	mamaMia->Receive(rBuffer.data(), rBuffer.size(), revieved);

	std::copy(rBuffer.begin(), rBuffer.begin() + revieved, std::back_inserter(recieved));
	
	this->m_tweet->SetText(recieved);

	if (recieved.compare("Your friends have no posts") != 0 && recieved.compare("You have no friends!") != 0)
	{
		std::array<char, 512> rBuffer2;
		int revieved2;
		std::string recieved2;
		mamaMia->Receive(rBuffer2.data(), rBuffer2.size(), revieved2);

		std::copy(rBuffer2.begin(), rBuffer2.begin() + revieved2, std::back_inserter(recieved2));

		this->m_tweet->SetUsername(recieved2);
	}
	else
		this->m_tweet->SetUsername("System");

}

FeedState::~FeedState()
{
	delete this->m_button.LogoButton;
	delete this->m_button.Profile;
	delete this->m_button.Tweet;
	delete this->m_button.Next;
	delete this->m_button.Prev;
	delete this->m_button.AddFriend;
	delete this->m_button.FriendName;
	delete this->AddFriendTextBox;
}

void FeedState::endState()
{
	std::cout << "Ending feedState";
}

void FeedState::updateKeybinds(const float& dt)
{
	this->checkforQuit();
}

void FeedState::updateButtons()
{
	this->m_button.LogoButton->update(m_mousePosView);
	this->m_button.Profile->update(m_mousePosView);
	this->m_button.Tweet->update(m_mousePosView);
	this->m_button.Next->update(m_mousePosView);
	this->m_button.Prev->update(m_mousePosView);
	this->m_button.AddFriend->update(m_mousePosView);

	this->m_button.FriendName->update(m_mousePosView);

}

void FeedState::updateUseButtons()
{

	if (this->m_button.LogoButton->getButtonState() == 2 and m_buttonRelease==1)
	{
		std::string username;
		ClientFeatures::Network en = ClientFeatures::Refresh;

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
			if (recieved.compare("Your friends have no posts") != 0 && recieved.compare("You have no friends!") != 0)
			{
				std::array<char, 512> rBuffer2;
				int revieved2;
				std::string recieved2;
				mamaMia->Receive(rBuffer2.data(), rBuffer2.size(), revieved2);

				std::copy(rBuffer2.begin(), rBuffer2.begin() + revieved2, std::back_inserter(recieved2));

				this->m_tweet->SetUsername(recieved2);
			}
			else
				this->m_tweet->SetUsername("System");
		}
		m_buttonRelease = 0;
	}	
	if (this->m_button.Profile->getButtonState() == 2 and m_buttonRelease == 1)
	{
			std::string username;
			ClientFeatures::Network en = ClientFeatures::Profile;

			uint16_t aux = static_cast<uint16_t>(en);
			std::cout << aux << '\n';

			bool result = mamaMia->Send((char*)&aux, sizeof(char));
			if (result)
			{
				this->m_states->push(new ProfileState(this->m_window, this->m_states, this->mamaMia));
				std::cout << "salut de la profile\n";
				
			}
			m_buttonRelease = 0;
	}	
	if (this->m_button.Tweet->getButtonState() == 2 and m_buttonRelease == 1)
	{	
			this->m_states->push(new TweetState(this->m_window, this->m_states, mamaMia));
			std::cout << "salut de la tweet\n";
			m_buttonRelease = 0;
	}
	if (this->m_button.Next->getButtonState() == 2 and m_buttonRelease == 1)
	{
		std::string username;
		ClientFeatures::Network en = ClientFeatures::NextPost;

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

			if (recieved.compare("There are no other posts") != 0)
			{
				std::array<char, 512> rBuffer2;
				int revieved2;
				std::string recieved2;
				mamaMia->Receive(rBuffer2.data(), rBuffer2.size(), revieved2);

				std::copy(rBuffer2.begin(), rBuffer2.begin() + revieved2, std::back_inserter(recieved2));

				this->m_tweet->SetUsername(recieved2);
			}
			else
				this->m_tweet->SetUsername("System");
		}
		m_buttonRelease = 0;
	}
	if (this->m_button.Prev->getButtonState() == 2 and m_buttonRelease == 1)
	{
		std::string username;
		ClientFeatures::Network en = ClientFeatures::PreviousPost;

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

			if (recieved.compare("There are no other posts") != 0)
			{
				std::array<char, 512> rBuffer2;
				int revieved2;
				std::string recieved2;
				mamaMia->Receive(rBuffer2.data(), rBuffer2.size(), revieved2);

				std::copy(rBuffer2.begin(), rBuffer2.begin() + revieved2, std::back_inserter(recieved2));

				this->m_tweet->SetUsername(recieved2);
			}
			else
				this->m_tweet->SetUsername("System");
		}
		m_buttonRelease = 0;
	}
	if (this->m_button.FriendName->getButtonState() == 2 and m_buttonRelease == 1)
	{
		this->AddFriendTextBox->SetSelected(true);
		m_buttonRelease = 0;
	}
	if (this->m_button.AddFriend->getButtonState() == 2 and m_buttonRelease == 1)
	{
		std::cout << "salut de la add friend\n";

		std::string username;
		ClientFeatures::Network en = ClientFeatures::AddFriend;

		uint16_t aux = static_cast<uint16_t>(en);
		std::cout << aux << '\n';

		if (this->AddFriendTextBox->GetText().size())
		{
			bool result = mamaMia->Send((char*)&aux, sizeof(char));
			if (result)
			{
				username = this->AddFriendTextBox->GetText();
				std::cout << username << "\n";
				mamaMia->Send(username.c_str(), username.size());

				std::array<char, 512> rBuffer;
				int revieved;
				std::string recieved;
				mamaMia->Receive(rBuffer.data(), rBuffer.size(), revieved);
				std::copy(rBuffer.begin(), rBuffer.begin() + revieved, std::back_inserter(recieved));

				if (recieved == "Added friend")
				{
					std::cout << "Merge\n";

				}
				else if (recieved == "Error")
					std::cout << "There is no user with that username\n";
				else
				{
					std::cout << "You are already a friend of that user";
				}


			}
		}
		m_buttonRelease = 0;
	}
}

void FeedState::update(const float& dt)
{
	this->updateButtons();
	this->updateUseButtons();
	this->m_tweet->updateTweetbox(m_mousePosView);
	this->updateMousePos();
	this->updateKeybinds(dt);
	
}

void FeedState::renderLines(sf::RenderTarget* target)
{
	//static lines
	target->draw(m_lineLeft);
	target->draw(m_lineRight);
	target->draw(m_lineTitle);
	//
}

void FeedState::renderButtons(sf::RenderTarget* target)
{
	this->m_button.LogoButton->render(target);
	this->m_button.Profile->render(target);
	this->m_button.Tweet->render(target);
	this->m_button.Next->render(target);
	this->m_button.Prev->render(target);
	this->m_button.AddFriend->render(target);
	this->m_button.FriendName->render(target);

	this->AddFriendTextBox->DrawTo(target);
}

void FeedState::renderTweetbox(sf::RenderTarget* target)
{
	m_tweet->render(target);
}

void FeedState::renderDesign(sf::RenderTarget* target)
{
	sf::Vertex rect1[] =
	{
	sf::Vertex(sf::Vector2f(target->getSize().x / 6, target->getSize().y / 10),sf::Color::Black),
	sf::Vertex(sf::Vector2f(target->getSize().x / 6 * 5-1, target->getSize().y / 10),sf::Color::Black),
	sf::Vertex(sf::Vector2f(target->getSize().x / 6 * 5-1, target->getSize().y/20*11),this->m_color.LightGray),
	sf::Vertex(sf::Vector2f(target->getSize().x / 6, target->getSize().y/20*11),this->m_color.LightGray)
	};
	
	sf::Vertex rect2[] =
	{
	sf::Vertex(sf::Vector2f(target->getSize().x / 6, target->getSize().y /20*11),this->m_color.LightGray),
	sf::Vertex(sf::Vector2f(target->getSize().x / 6 * 5-1, target->getSize().y / 20*11),this->m_color.LightGray),
	sf::Vertex(sf::Vector2f(target->getSize().x / 6 * 5-1, target->getSize().y),sf::Color::Black),
	sf::Vertex(sf::Vector2f(target->getSize().x / 6, target->getSize().y),sf::Color::Black)
	};

	target->draw(rect1, 4, sf::Quads);
	target->draw(rect2, 4, sf::Quads);

	target->draw(TwitterLogoSprite);

}

void FeedState::render(sf::RenderTarget* target)
{
	if (!target)
		target = this->m_window;

	renderLines(target);
	renderDesign(target);
	renderButtons(target);
	renderTweetbox(target);
}



void FeedState::setColorLines(const sf::Color& color)
{
	m_lineLeft[0].color = color;
	m_lineLeft[1].color = color;
	m_lineRight[0].color = color;
	m_lineRight[1].color = color;
	m_lineTitle[0].color = color;
	m_lineTitle[1].color = color;

}

void FeedState::initLines(sf::RenderTarget* target)
{
	this->m_lineLeft = sf::VertexArray(sf::Lines, 2);
	this->m_lineRight = sf::VertexArray(sf::Lines, 2);
	this->m_lineTitle = sf::VertexArray(sf::Lines, 2);
	this->m_lineLeft[0] = sf::Vector2f(target->getSize().x /6, 0);
	this->m_lineLeft[1] = sf::Vector2f(target->getSize().x / 6, target->getSize().y);
	this->m_lineRight[0] = sf::Vector2f(target->getSize().x / 6 * 5, 0);
	this->m_lineRight[1] = sf::Vector2f(target->getSize().x / 6 * 5, target->getSize().y);
	this->m_lineTitle[0] = sf::Vector2f(target->getSize().x / 6, target->getSize().y / 10);
	this->m_lineTitle[1] = sf::Vector2f(target->getSize().x / 6 * 5, target->getSize().y / 10);

	this->m_lineLeft[0].color = this->m_color.DarkGray;
	this->m_lineLeft[1].color = this->m_color.DarkGray;
	this->m_lineRight [0] .color = this->m_color.DarkGray;
	this->m_lineRight[1].color = this->m_color.DarkGray;
	this->m_lineTitle[0].color = this->m_color.DarkGray;
	this->m_lineTitle[1].color = this->m_color.DarkGray;
}

void FeedState::initButtons(sf::RenderTarget* target)
{
	this->m_button.LogoButton = new Button(target->getSize().x / 48, target->getSize().y / 30,
		target->getSize().x / 8, target->getSize().y / 8, &this->m_font, "Refresh Feed",
		this->m_color.Black, this->m_color.DarkGray,
		this->m_color.Blue, this->m_color.Black);

	this->m_button.FriendName = new Button(target->getSize().x / 48, target->getSize().y / 30 * 8,
		target->getSize().x / 8, target->getSize().y / 8, &this->m_font, "",
		this->m_color.Black, this->m_color.DarkGray,
		this->m_color.Blue, this->m_color.Black);

	this->AddFriendTextBox = new Textbox(15, this->m_color.ExtraExtraLightGray, true);
	this->AddFriendTextBox->SetFont(m_font);
	float x, y;
	x = target->getSize().x / 48;
	y = target->getSize().y / 30 * 8;
	this->AddFriendTextBox->SetPosition({ x, y });

	this->m_button.AddFriend = new Button(target->getSize().x / 48, target->getSize().y / 30 * 12,
		target->getSize().x / 8, target->getSize().y / 8, &this->m_font, "Add Friend",
		this->m_color.Black, this->m_color.DarkGray,
		this->m_color.Blue, this->m_color.Black);

	this->m_button.Profile = new Button(target->getSize().x / 48*41, target->getSize().y / 30*27,
		target->getSize().x / 8, target->getSize().y / 20, &this->m_font, "Profile",
		this->m_color.Black, this->m_color.DarkGray,
		this->m_color.Blue, this->m_color.Black);


	this->m_button.Tweet = new Button(target->getSize().x / 48, target->getSize().y/30*25,
		target->getSize().x / 8, target->getSize().y / 8, &this->m_font, "Tweet",
		this->m_color.Blue, this->m_color.DarkGray,
		this->m_color.ExtraExtraLightGray, this->m_color.Black);

	this->m_button.Next = new Button(target->getSize().x / 12 * 8, target->getSize().y / 12 * 10,
		target->getSize().x / 12, target->getSize().y / 20,
		&this->m_font, "Next",
		sf::Color::Black, this->m_color.LightGray,
		this->m_color.Blue, sf::Color::Black);

	this->m_button.Prev = new Button(target->getSize().x / 4, target->getSize().y / 12 * 10,
		target->getSize().x / 12, target->getSize().y / 20,
		&this->m_font, "Previous",
		sf::Color::Black, this->m_color.LightGray,
		this->m_color.Blue, sf::Color::Black);
}

void FeedState::initTweetbox(sf::RenderTarget* target)
{
	this->m_tweet = new Tweetbox(target->getSize().x/48*9,target->getSize().y/4,
		sf::Vector2f(target->getSize().x/48*29,target->getSize().y/2), 50, 100,
		&this->m_font, sf::Color::Black,
		"Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat mas");
	this->m_tweet->initButtons();
	
}

void FeedState::initSpritesTextures(sf::RenderTarget* target)
{
	this->TwitterLogo.loadFromFile("../images2/twitter.jpg");
	this->TwitterLogo.setSmooth(true);
	this->TwitterLogoSprite.setScale(sf::Vector2f(0.15, 0.15));
	this->TwitterLogoSprite.setPosition(sf::Vector2f(target->getSize().x/12*11-100,
		50));
	this->TwitterLogoSprite.setTexture(TwitterLogo);
}

void FeedState::SetText(const sf::Event& input)
{
	if (this->AddFriendTextBox->IsSelected())
		this->AddFriendTextBox->Type(input);
}
