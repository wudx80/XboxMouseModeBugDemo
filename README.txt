Purpose

This code repo is to demo issue we met when using Xbox mouse mode in Windows 10 sdk, Build 14393.
We want to build a library which show some UI to end user, our UI must enable mouse mode.

As some app developer would turn off mouse mode in their app, we should set the RequiresPointer of
our root page to RequiresPointer.WhenFocused.
Then the buttons inside the page would not work properly randomly.

Here I have found a reproduce path of the issue: (Here Click means press "A" key on control)
1. Launch the demo app in Xbox device, no mouse pointer in start page.
2. Click the "Show Content" button.
3. Then the content page showed with mouse pointer. That is a webview to Google as well as 3 buttons on top of it.
4. Click the "Mute" button, its text changes to "Unmute".
5. Move mouse over the "CTA" button, but don't click it.
6. Move and click the "Close" button, that should close the content and back to start page, but it won't.
7. Continue click many times, it would randomly back to start page.

If you stuck in step 7, you can try this method to get out:
click the search text of google's page, the input keyboard is showed,
press "B" key 2 times, then "Close" button is in selected mode,
move and click "Close" button, then you will be back to start page.
