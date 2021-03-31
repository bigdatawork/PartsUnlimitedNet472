---
lab:
    title: 'Text Analytics'
---

# AI-900 Labs
## Get latest files

1.  Start Visual Studio Code (the program icon is pinned to the bottom taskbar). When it opens, you should see the MSLEARN-AI900 project on the left-hand panel.

![Visual Studio Code Icon](./images/vscode.jpg)

2.  We will pull the latest version of the project. In the open terminal type +++getfiles.cmd+++ and press **enter**. This command pulls the latest version of the project to your folder. 
3.  Once the command runs, you can close the terminal panel. Now you can begin the lab. 

![Support image for using terminal in Visual Studio Code.](./images/terminal_support1.jpg)

In this lab we will create an application that can understand language.

-  Open the **07 - Text Analytics.ipynb** notebook in Visual Studio Code.
    **Note:** You may be prompted to complete a 2-minute survey. Go ahead and select **No, thanks**. You may need to do this more than once.
-  Follow the instructions in the notebook to complete the lab.

# Text Analytics

Natural Language Processing (NLP) is a branch of artificial intelligence (AI) that deals with written and spoken language. You can use NLP to build solutions that extracting semantic meaning from text or speech, or that formulate meaningful responses in natural language.

Microsoft Azure *cognitive services* includes the *Text Analytics* service, which provides some out-of-the-box NLP capabilities, including the identification of key phrases in text, and the classification of text based on sentiment.

![A robot reading a notebook](./images/NLP.jpg)

For example, suppose the fictional *Margie's Travel* organization encourages customers to submit reviews for hotel stays. You could use the Text Analytics service to summarize the reviews by extracting key phrases, determine which reviews are positive and which are negative, or analyze the review text for mentions of known entities such as locations or people.

## View Review Documents

Let's start by taking a look at some hotel reviews that have been left by customers.

The reviews are in text files. To see them, just run the code below by clicking the **Run cell** (&#9655;) button to the left of the cell.

```
# Text Analytics

Natural Language Processing (NLP) is a branch of artificial intelligence (AI) that deals with written and spoken language. You can use NLP to build solutions that extracting semantic meaning from text or speech, or that formulate meaningful responses in natural language.

Microsoft Azure *cognitive services* includes the *Text Analytics* service, which provides some out-of-the-box NLP capabilities, including the identification of key phrases in text, and the classification of text based on sentiment.

![A robot reading a notebook](./images/NLP.jpg)

For example, suppose the fictional *Margie's Travel* organization encourages customers to submit reviews for hotel stays. You could use the Text Analytics service to summarize the reviews by extracting key phrases, determine which reviews are positive and which are negative, or analyze the review text for mentions of known entities such as locations or people.

## View Review Documents

Let's start by taking a look at some hotel reviews that have been left by customers.

The reviews are in text files. To see them, just run the code below by clicking the **Run cell** (&#9655;) button to the left of the cell.
```
## Create a Cognitive Services Resource

To analyze the text in these reviews, you can use the **Text Analytics** cognitive service. To use this, you need to provision either a **Text Analytics** or **Cognitive Services** resource in your Azure subscription (Use a Text Analytics resource if this is the only service you plan to use or you want to track its usage separately; otherwise you can use a Cognitive Services resource to combine the Text Analytics service with other cognitive services - enabling developers to use a single endpoint and key to access them.)

If you don't already have one, use the following steps to create a **Cognitive Services** resource in your Azure subscription:

> **Note**: If you already have a Cognitive Services resource, just open its **Quick start** page in the Azure portal and copy its key and endpoint to the cell below. Otherwise, follow the steps below to create one.

1. In another browser tab, open the Azure portal at https://portal.azure.com, signing in with your Microsoft account.
2. Click the **&#65291;Create a resource** button, search for *Cognitive Services*, and create a **Cognitive Services** resource with the following settings:
    - **Subscription**: *Your Azure subscription*.
    - **Resource group**: *Select or create a resource group with a unique name*.
    - **Region**: *Choose any available region*:
    - **Name**: *Enter a unique name*.
    - **Pricing tier**: S0
    - **I confirm I have read and understood the notices**: Selected.
3. Wait for deployment to complete. Then go to your cognitive services resource, and on the **Overview** page, click the link to manage the keys for the service. You will need the endpoint and keys to connect to your cognitive services resource from client applications.

### Get the Key and Endpoint for your Cognitive Services resource

To use your cognitive services resource, client applications need its  endpoint and authentication key:

1. In the Azure portal, on the **Keys and Endpoint** page for your cognitive service resource, copy the **Key1** for your resource and paste it in the code below, replacing **YOUR_COG_KEY**.
2. Copy the **endpoint** for your resource and and paste it in the code below, replacing **YOUR_COG_ENDPOINT**.
3. Run the code in the cell below by clicking its green <span style="color:green">&#9655;</span> button.

```
cog_key = 'YOUR_COG_KEY'
cog_endpoint = 'YOUR_COG_ENDPOINT'

print('Ready to use cognitive services at {} using key {}'.format(cog_endpoint, cog_key))
```

## Detect Language
Let's start by identifying the language in which these reviews are written.

```
import os
from azure.cognitiveservices.language.textanalytics import TextAnalyticsClient
from msrest.authentication import CognitiveServicesCredentials

# Get a client for your text analytics cognitive service resource
text_analytics_client = TextAnalyticsClient(endpoint=cog_endpoint,
                                            credentials=CognitiveServicesCredentials(cog_key))

# Analyze the reviews you read from the /data/reviews folder earlier
language_analysis = text_analytics_client.detect_language(documents=reviews)

# print detected language details for each review
for review_num in range(len(reviews)):
    # print the review id
    print(reviews[review_num]['id'])

    # Get the language details for this review
    lang = language_analysis.documents[review_num].detected_languages[0]
    print(' - Language: {}\n - Code: {}\n - Score: {}\n'.format(lang.name, lang.iso6391_name, lang.score))

    # Add the detected language code to the collection of reviews (so we can do further analysis)
    reviews[review_num]["language"] = lang.iso6391_name
```

## Extract Key Phrases

Now you can analyze the text in the customer reviews to identify key phrases that give some indication of the main talking points.

```
# # Use the client and reviews you created in the previous code cell to get key phrases
key_phrase_analysis = text_analytics_client.key_phrases(documents=reviews)

# print key phrases for each review
for review_num in range(len(reviews)):
    # print the review id
    print(reviews[review_num]['id'])

    # Get the key phrases in this review
    print('\nKey Phrases:')
    key_phrases = key_phrase_analysis.documents[review_num].key_phrases
    # Print each key phrase
    for key_phrase in key_phrases:
        print('\t', key_phrase)
    print('\n')
```

The key phrases can help you gain an understanding of the most important talking points in each review. For example, a review containing a phrase "helpful staff" or "poor service" can give you an indication of some of the main concerns of the reviewer.

## Determine Sentiment

It might be useful to classify the reviews as *positive* or *negative* based on a *sentiment score*. Again, you can use the Text Analytics service to do this.

```
# Use the client and reviews you created previously to get sentiment scores
sentiment_analysis = text_analytics_client.sentiment(documents=reviews)

# Print the results for each review
for review_num in range(len(reviews)):

    # Get the sentiment score for this review
    sentiment_score = sentiment_analysis.documents[review_num].score

    # classifiy 'positive' if more than 0.5, 
    if sentiment_score < 0.5:
        sentiment = 'negative'
    else:
        sentiment = 'positive'

    # print file name and sentiment
    print('{} : {} ({})'.format(reviews[review_num]['id'], sentiment, sentiment_score))
```

## Extract Known Entities

*Entities* are things that might be mentioned in text that reference some commonly understood type of item. For example, a location, a person, or a date. Let's suppose you're interested in dates and places mentioned in the reviews - you can use the following code to find them.

```
# Use the client and reviews you created previously to get named entities
entity_analysis = text_analytics_client.entities(documents=reviews)

# Print the results for each review
for review_num in range(len(reviews)):
    print(reviews[review_num]['id'])
    # Get the named entitites in this review
    entities = entity_analysis.documents[review_num].entities
    for entity in entities:
        # Only get location entitites
        if entity.type in ['DateTime','Location']:
            link = '(' + entity.wikipedia_url + ')' if entity.wikipedia_id is not None else ''
            print(' - {}: {} {}'.format(entity.type, entity.name, link))
```

Note that some entities are sufficiently well-known to have an associated Wikipedia page, in which case the Text Analytics service returns the URL for that page.

## Learn More

For more information about the Text Analytics service, see [the Text Analytics service documentation](https://docs.microsoft.com/azure/cognitive-services/text-analytics/)
