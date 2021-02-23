VAR metBob = false
VAR metJen = false
VAR metJoe = false
VAR metKris = false
VAR bobSuggestedKris = false

-> Explore

=== Explore ===
#explore
+ [chat:bob] -> ChatBob
+ [chat:jen] -> ChatJen
+ [chat:joe] -> ChatJoe
+ [chat:kris] -> ChatKris

=== ChatBob ===
#chat:bob
{not metBob: 
  ~metBob = true
  Hi Stranger. My name is Bob. How are you doing?
-else:
  Hello again friend. Still doing well I hope?
}
+ [Good]
  I hope you're finding this basement interesting.
  ++ [Yes]
    Nice. We do our best to entertain down here.
  ++ [No]
    Well perhaps you should take a look around.
    There's often some interesting gossip around nere.
+ [Bored]
  {not metKris:
    Really? you should chat with Kris. She can be quite amusing.
    She's hiding in the shadows.
    ~bobSuggestedKris = true
  - else:
    I'm surprized Kris didn't entertain you.
  }
- -> Explore

=== ChatJen ===
#chat:jen
{not metJen:
  ~metJen = true
  Greetings traveler, my name is Jen.
-else:
  Hello again friend.
}
+ [Bye] -> Explore

=== ChatJoe ===
#chat:joe
{not metJoe:
  ~metJoe = true
  Hello there, I'm Joe. Pleased to meet you.
-else:
  Good to see you again.
}
+ [Bye] -> Explore

=== ChatKris ===
#chat:kris
{not metKris:
  ~metKris = true
  Hi there hansome.
  {bobSuggestedKris: Did Bob send you my way? He sometimes sends newcomers to me!}
-else:
  Welcome back hansome.
}
+ [Bye] -> Explore
