#pragma once
#include "State.h"
#include "TweetState.h"
#include"ProfileState.h"


class FeedState : public State
{
private:
	struct TwitterHomeButton
	{
		Button* LogoButton;
		Button* Profile;
		Button* Tweet;

		Button* Next;
		Button* Prev;

		Button* FriendName;
		Button* AddFriend;
	};
	TwitterHomeButton m_button;


		sf::Texture TwitterLogo;
		sf::Sprite TwitterLogoSprite;


	Textbox* AddFriendTextBox;

	Tweetbox* m_tweet;

	sf::VertexArray m_lineLeft;
	sf::VertexArray m_lineRight;
	sf::VertexArray m_lineTitle;

	std::shared_ptr<TcpSocket> mamaMia;
public:
	FeedState(sf::RenderWindow* window, std::stack<State*> *states, std::shared_ptr<TcpSocket> socket);
	virtual ~FeedState();

	void endState();
	void updateKeybinds(const float& dt);
	void updateButtons();
	void updateUseButtons();
	void update(const float& dt);

	void renderLines(sf::RenderTarget* target);
	void renderButtons(sf::RenderTarget* target);
	void renderTweetbox(sf::RenderTarget* target);
	void renderDesign(sf::RenderTarget* target);
	void render(sf::RenderTarget* target = NULL);

	//design
	void setColorLines(const sf::Color& color);
	void initLines(sf::RenderTarget* target);
	void initButtons(sf::RenderTarget* target);
	void initTweetbox(sf::RenderTarget* target);
	void initSpritesTextures(sf::RenderTarget* target);
	

	void SetText(const sf::Event& input);
};

