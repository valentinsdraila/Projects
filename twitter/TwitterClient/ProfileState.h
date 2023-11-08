#pragma once
#include "State.h"
#include "FeedState.h"
class ProfileState : public State
{
private:
	struct TwitterProfileButtons {
		Button* Back;
		Button* NextTweet;
		Button* PrevTweet;
	};
	TwitterProfileButtons m_button;

	sf::RectangleShape m_profileBox;

	sf::VertexArray m_nameLine;
	sf::VertexArray m_bioLine;

	Tweetbox* m_tweet;
	
	std::shared_ptr<TcpSocket> mamaMia;


public:
	ProfileState(sf::RenderWindow* window, std::stack<State*>* states, std::shared_ptr<TcpSocket> socket);
	virtual ~ProfileState();


	void endState();
	void updateKeybinds(const float& dt);
	void updateButtons();
	void updateUseButtons();
	void update(const float& dt);

	void renderButtons(sf::RenderTarget* target);
	void renderBox(sf::RenderTarget* target);
	void renderLines(sf::RenderTarget* target);
	void renderTweetbox(sf::RenderTarget* target);
	void render(sf::RenderTarget* target = NULL);


	void initButtons(sf::RenderTarget* target);
	void initBox(sf::RenderWindow* target);
	void initLines(sf::RenderWindow* target);
	void initTweetbox(sf::RenderTarget* target);


};
