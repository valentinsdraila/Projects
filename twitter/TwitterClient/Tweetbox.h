#pragma once
#include <SFML/Graphics.hpp>
#include "Button.h"
class Tweetbox : public sf::Shape
{
private:
	sf::Vector2f m_size;
	float m_radius;
	unsigned int m_cornerPointCount;

	sf::Font* m_font;
	sf::Text m_text;
	sf::Color m_color;


	struct TweetboxButtons {
		Button* UserName;
		Button* Likes;
		Button* Comment;
		Button* Retweet;
	};
	TweetboxButtons m_button;

	struct Clicked
	{
		bool UserNameClick;
		bool LikesClick;
		bool CommentClick;
		bool RetweetClick;
	};
	Clicked m_click;

	struct TweetboxTextures
	{
		sf::Texture Like;
		sf::Texture Retweet;
		sf::Texture Comment;
		sf::Texture UserImg;
	};
	TweetboxTextures m_texture;

	struct TweetboxSprites
	{
		sf::Sprite LikeSprite;
		sf::Sprite RetweetSprite;
		sf::Sprite CommentSprite;
		sf::Sprite UserImgSprite;
	};
	TweetboxSprites m_sprite;

	sf::RectangleShape m_likeLine;
	sf::RectangleShape m_retweetLine;

public:
	Tweetbox();

	void SetText(const std::string& text);

	Tweetbox(float x, float y, const sf::Vector2f& size,
		float radius, unsigned int cornerPointCount,
		sf::Font* font, sf::Color color, std::string text = "");
	~Tweetbox();
	void SetUsername(const std::string& username);

	void setSize(const sf::Vector2f& size);
	void setCornersRadius(float radius);
	void setCornerPointCount(unsigned int count);

	const sf::Vector2f& getSize() const;
	float getCornersRadius() const;
	virtual std::size_t getPointCount() const;
	virtual sf::Vector2f getPoint(std::size_t index) const;

	void reboundText();

	void updateUseButtons();
	void updateButtons(const sf::Vector2f mousePos);
	void updateTweetbox(const sf::Vector2f mousePos);

	void renderButtons(sf::RenderTarget* target);
	void renderSpritesTextures(sf::RenderTarget* target);
	void renderLines(sf::RenderTarget* target);
	void render(sf::RenderTarget* target);

	void initButtons();
	void initClick();
	void initSpritesTextures();
	void initLines();

};

