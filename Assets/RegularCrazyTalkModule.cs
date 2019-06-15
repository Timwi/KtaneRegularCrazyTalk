using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RegularCrazyTalk;
using UnityEngine;

using Rnd = UnityEngine.Random;

/// <summary>
/// On the Subject of Regular Crazy Talk
/// Created by Timwi, Ryaninator and the community at large
/// </summary>
public class RegularCrazyTalkModule : MonoBehaviour
{
    public KMBombInfo Bomb;
    public KMBombModule Module;
    public KMAudio Audio;
    public KMRuleSeedable RuleSeedable;
    public TextMesh TextMesh;
    public Transform TextMeshParent;
    public Renderer TextRenderer;
    public Renderer SurfaceRenderer;
    public TextMesh DigitDisplay;

    public KMSelectable ButtonUp, ButtonDown, ButtonScreen;

    private static int _moduleIdCounter = 1;
    private int _moduleId;
    private bool _isSolved;
    private Func<PotentialPhraseAction>[] _phraseGenerators;
    private List<PhraseAction> _phraseActions;
    private int _selectedPhraseIx;
    private int _timeWhenHeld;

    public static readonly string[] _phrases = new[]
    {
            "We just blew up.",
            "We ran out of time.",
            "You cut out.",
            "You just cut out.",
            "Were you saying something?",
            "Did you say something?",
            "I can’t hear you, you’re breaking up.",
            "You’re breaking up.",
            "Repeat?",
            "Please repeat.",
            "[A] batteries in [B] holders.",
            "Forget Me Not stage [A] is a [B].",
            "No Christmas crackers.",
            "Don’t wash tennis balls.",
            "There’s no decoy.",
            "How do I know which one’s the decoy?",
            "Decoy is [A: Rock|Paper|Scissors|Lizard|Spock].",
            "How the heck am I supposed to pronounce this?",
            "Black is on the [A: left|right], no poop.",
            "Mind the gap.",
            "Honk honk.",
            "You have violated an area protected by a security system.",
            "Welcome to Coffeebucks, may I take your name please?",
            "[A: Point of Order|Poker|Blackjack] is [B: Ace|Two|Three|Four|Five|Six|Seven|Eight|Nine|Ten|Jack|Queen|King] of [C: Spades|Hearts|Clubs|Diamonds].",
            "I need [A: Big Circle|Blind Maze|Combination Lock|Laundry|Press X] for [B] solved.",
            "I forgot we have a [A: Turn the Key|Forget Me Not|Forget Everything|Souvenir].",
            "I forgot to mention we have a [A: Turn the Key|Forget Me Not|Forget Everything|Souvenir].",
            "Do I turn the right key?",
            "What does a [A: parallel|serial|DVI|DVI-D|PS/2|RCA|Stereo RCA|RJ|RJ-45|USB] port look like again?",
            "What do you call the [A: small square port|small round port|port with two holes|empty module] again?",
            "Hold on, I have a phone call.",
            "Hold on, I’m getting a phone call.",
            "Hold on, I’m doing [A: Turn the Key|a Battleship|English Test|Digital Root|Mastermind|Minesweeper|Anagrams|Word Scramble|a Swan|the needy|push-ups].",
            "Never mind.",
            "Quiet, I’m preparing Safety Safe.",
            "Quiet please, I’m preparing Safety Safe.",
            "Please be quiet, I’m preparing Safety Safe.",
            "Does this version count for Turn the Keys?",
            "Why is there vanilla on this bomb?",
            "Whoops, I hit a wall.",
            "I’m gonna kill this bomb.",
            "That was a fake strike, don’t worry.",
            "Tell me when to initiate.",
            "How do I know if it’s [A: Blind Alley or Tap Code|Colored or Uncolored Squares|Anagrams or Word Scramble|Simon Screams or Shrieks|Crazy Talk or Regular|Regular Crazy Talk or non|Beach or Waterfall|papers or sapper|Sonic & Knuckles or just Sonic|vanilla or translated]?",
            "Hang on, gotta wait for an even minute.",
            "I think this is [A: Taxi Dispatch|Extractor Fan|Train Station|Arcade|Casino|Supermarket|Soccer Match|Tawny Owl|Sewing Machine|Thrush Nightingale|Car Engine|Reloading Glock|Oboe|Saxophone|Tuba|Marimba|Phone Ringing|Tibetan Nuns|Throat Singing|Beach|Dial-up Internet|Police Radio Scanner|Censorship Bleep|Medieval Weapons|Door Closing|a bug|Chainsaw|Compressed Air|Servo Motor|Waterfall|Tearing Fabric|Zipper|Vacuum Cleaner|Ballpoint Pen Writing|Rattling Iron Chain|Book Page Turning|Table Tennis|Squeeky Toy|Helicopter|Firework Exploding|Glass Shattering].",
            "It’s [A: Johnny Cage|Kano|Liu Kang|Raiden|Scorpion|Sonya|Sub-Zero] versus [B: Johnny Cage|Kano|Liu Kang|Raiden|Scorpion|Sonya|Sub-Zero].",
            "Text Field is [A: Alfa|Bravo|Charlie|Delta|Echo|Foxtrot|easy].",
            "Here I’ll post a log.",
            "Where do you find the logfile again?",
            "Wait, hold on, let’s do another module first.",
            "Maritime Flags is going by too fast.",
            "You Are One.",
            "You Are One, three words.",
            "You Are One, two letters and a number.",
            "You Are One, with the NATO.",
            "U R 1.",
            "U R 1, three words.",
            "U R 1, with the NATO.",
            "Uniform Romeo 1.",
            "Uniform Romeo 1, three words.",
            "Uniform Romeo 1, with the NATO.",
            "[A: Morse Code|Morse-A-Maze|Reverse Morse|Color Morse|The Cube rotations|Flashing Lights|Simon Sends], waiting for the reset.",
            "Your.",
            "Your, Why Oh You Are.",
            "You’re.",
            "Your apostrophe.",
            "You’re apostrophe.",
            "Your possessive.",
            "You’re possessive.",
            "You are words.",
            "UR words.",
            "The game crashed.",
            "The game just crashed.",
            "Oops, the game crashed.",
            "Oops, the game crashed. Literally unplayable!",
            "What are the numbers for The Swan again?",
            "I missed a Swan reset.",
            "Letters on Swan.",
            "",
            "It displays nothing.",
            "It displays nothing at all.",
            "It displays literally nothing.",
            "It literally displays nothing.",
            "Literally nothing.",
            "It’s blank.",
            "It’s literally blank.",
            "Literally blank.",
            "It’s actually blank.",
            "Exactly what it says.",
            "Exectly what it says.",
            "This is exactly what it says.",
            "Exactly what is says. Exactly is misspelled.",
            "This is exactly what it says: exactly what it says.",
            "That’s what it says.",
            "That’s what the module says.",
            "Yeah, that’s what it says.",
            "Yeah, that’s what the module says.",
            "No, that’s what it says.",
            "No, that’s what the module says.",
            "No no no, that’s what the module says.",
            "The buttons don’t do anything.",
            "Who’s the one with the loud keyboard?",
            "Please mute yourself.",
            "You should mute yourself.",
            "Are we friends on [A: Steam|Discord|Facebook|MySpace|Skype]?",
            "I have an idea for a new module.",
            "I have a great idea for a new module.",
            "I have an idea for a new needy module.",
            "I forgot to enable your profile.",
            "Oops, I forgot to enable your profile.",
            "So what profiles are we using?",
            "It’s still loading.",
            "Hold on, it’s still loading.",
            "Hold on, the lights just went out.",
            "Hold on, gotta turn off the alarm clock.",
            "Gotta turn off the alarm clock.",
            "[A: ABC|ABD|ABH|ACD|ACH|ADH|BCD|BCH|BDH|CDH].",
            "My letters are [A: ABC|ABD|ABH|ACD|ACH|ADH|BCD|BCH|BDH|CDH].",
            "3D Maze, my letters are [A: ABC|ABD|ABH|ACD|ACH|ADH|BCD|BCH|BDH|CDH].",
            "Gridlock, [A: red|blue|green|yellow] star at [B: Alfa|Bravo|Charlie|Delta]-[C: 1|2|3|4], pressing next.",
            "Let me find the torus.",
            "Let me find a sphere.",
            "I thought this module was disabled.",
            "I thought I disabled [A: Forget Me Not|Forget Everything|Souvenir|Turn the Key|Turn the Keys|The Cube|Tax Returns|Laundry|needies|vanilla|the alarm clock].",
            "I thought I’d disabled [A: Forget Me Not|Forget Everything|Souvenir|Turn the Key|Turn the Keys|The Cube|Tax Returns|Laundry|needies|vanilla|the alarm clock].",
            "I thought I had disabled [A: Forget Me Not|Forget Everything|Souvenir|Turn the Key|Turn the Keys|The Cube|Tax Returns|Laundry|needies|vanilla|the alarm clock].",
            "Can you do [A: The Cube|The Sphere|Tax Returns|LEGO|Laundry|Black Hole|Jewel Vault|me a favor|Simon Sings|Simon Sends|Turtle Robot|3D Tunnels|Pattern Cube|me a favour|Splitting The Loot|Coffeebucks|Kudosudoku|Regular Crazy Talk]?",
            "We solved the bomb.",
            "We did it, we solved the bomb.",
            "We did it, bomb [A: disarmed|solved|defused|diffused].",
            "Do you wanna play [A: Fortnite|PUBG|CS:GO|Challenge & Contact|the piano]?",
            "Crazy Talk. All words. Quote the phrase the word stop twice end quote.",
            "Crazy Talk. Ready?",
            "Crazy Talk. Ready? Quote.",
            "Why is there a Regular Crazy Talk on this bomb?",
            "Is this Regular Crazy Talk or non?",
            "Not Regular. I meant Crazy Talk.",
            "Actually, it’s just Crazy Talk.",
            "I think this module has a bug.",
            "All available experts please report to room A-9.",
            "Emergency cleared. All experts report to your stations.",
            "All personnel please evacuate to your nearest pod and report to your supervisor.",
            "Contact.",
            "Challenge 3 2 1.",
            "Challenge three two one.",
            "Challenge. 3, 2, 1.",
            "Challenge. Three, two, one.",
            "She sells sea shells on the sea shore.",
            "She sells sea shells by the sea shore.",
            "Sea shells she sells on the sea shore.",
            "Sea shells she sells by the sea shore.",
            "It’s the one with the sea shells.",
            "Imagine an imaginary menagerie manager imagining managing an imaginary menagerie.",
            "Imagine an imaginary menagerie manager managing an imaginary menagerie.",
            "Imagine an imaginary menagerie manager imagining managing a menagerie.",
            "Imagine an imaginary menagerie managed by an imaginary menagerie manager.",
            "Imagine a menagerie manager imagining managing an imaginary menagerie.",
            "Imagine a menagerie manager imagining an imaginary menagerie.",
            "Imagine a menagerie managed by an imaginary menagerie manager.",
            "Imagine a menagerie managed by an imaginary menagerie manager imagining a menagerie.",
            "Imagine a menagerie managed by a menagerie manager imagining managing a menagerie.",
            "It’s the one with the menagerie manager.",
            "Any progress on [any module name]?",
            "Light Cycle is [!a sequence of six colors|6-6:red/green/blue/magenta/yellow/white].",
            "The Screw is [!a sequence of six colors|6-6:red/green/blue/magenta/yellow/white].",
            "[!a sequence of rhyming words|3-6:boat/coat/float/gloat/goat/moat/note/oat/quote/rote/stoat/throat/vote/wrote].",
            "Never mind, the module solved itself.",
            "Never mind, it solved itself.",
            "Never mind, Regular Crazy Talk solved itself.",
            "Wait, we have a [A: Forget Me Not|Forget Everything|Souvenir|Swan|Fast Math|needy].",
            "I missed stage [A] on [B: Forget Me Not|Forget Everything].",
            "What?",
            "What’s the correct phrase on Regular Crazy Talk?"
    };

    void Start()
    {
        _moduleId = _moduleIdCounter++;
        _isSolved = false;

        var rnd = RuleSeedable.GetRNG();
        Debug.LogFormat("[Regular Crazy Talk #{0}] Using rule seed: {1}", _moduleId, rnd.Seed);
        _phraseGenerators = new Func<PotentialPhraseAction>[_phrases.Length];

        var digits = Enumerable.Range(0, 10).ToArray();
        for (var phrIx = 0; phrIx < _phrases.Length; phrIx++)
        {
            rnd.ShuffleFisherYates(digits);
            var arr = digits.Subarray(0, 3);
            var getters = Enumerable.Range(0, 3).Select(i => new Func<PhraseArgPair[], int>((PhraseArgPair[] _) => arr[i])).ToArray();
            var phrase = RegularCrazyTalkModule._phrases[phrIx];
            var phrFmt = new StringBuilder();
            var argGenerators = new List<Func<PhraseArgPair>>();
            Match result;
            while ((result = Regex.Match(phrase, @"\[(?:([A-C])|([A-C]): ([^\]]*)|!([^\]\|]*)\|(\d+)-(\d+):([^\]]*?)|([^\]\|]+))\]")).Success)
            {
                var arg = argGenerators.Count;
                phrFmt.Append(phrase.Substring(0, result.Index));
                phrFmt.Append("{");
                phrFmt.Append(arg);
                phrFmt.Append("}");

                if (result.Groups[1].Success)
                {
                    argGenerators.Add(() =>
                    {
                        var value = Rnd.Range(0, 10);
                        return new PhraseArgPair(value.ToString(), value);
                    });
                    getters[result.Groups[1].Value[0] - 'A'] = pairs => (int) pairs[arg].Value;
                }
                else if (result.Groups[3].Success)
                {
                    var options = result.Groups[3].Value.Split('|');
                    var numbers = new List<int>();
                    while (numbers.Count < options.Length)
                        numbers.AddRange(Enumerable.Range(0, 10));
                    rnd.ShuffleFisherYates(numbers);
                    argGenerators.Add(() =>
                    {
                        var ix = Rnd.Range(0, options.Length);
                        return new PhraseArgPair(options[ix], numbers[ix]);
                    });
                    getters[result.Groups[2].Value[0] - 'A'] = pairs => (int) pairs[arg].Value;
                }
                else if (result.Groups[4].Success)
                {
                    // result.Groups[4].Value = “a sequence of ...”
                    Debug.LogFormat(@"<Regular Crazy Talk #{0}> min={1}, max={2}", _moduleId, result.Groups[5].Value, result.Groups[6].Value);
                    var min = int.Parse(result.Groups[5].Value);
                    var max = int.Parse(result.Groups[6].Value);
                    var words = result.Groups[7].Value.Split('/');
                    var nums = Enumerable.Range(0, words.Length).ToArray();
                    rnd.ShuffleFisherYates(nums);
                    var wordsToFindIxs = nums.Subarray(0, 3);    // words whose position in the phrase matter
                    argGenerators.Add(() =>
                    {
                        // Generate a sequence of (between min and max) words that definitely contains the words the player must look for
                        var seq = nums.ToList().Shuffle();
                        foreach (var tf in wordsToFindIxs)
                            seq.Remove(tf);
                        seq.InsertRange(0, wordsToFindIxs);
                        var nm = Rnd.Range(min, max + 1);
                        seq.RemoveRange(nm, seq.Count - nm);
                        seq.Shuffle();
                        return new PhraseArgPair(seq.Select(ix => words[ix]).Join(", "), wordsToFindIxs.Select(ix => seq.IndexOf(ix)).ToArray());
                    });
                    for (var optIxFor = 0; optIxFor < 3; optIxFor++)
                    {
                        var optIx = optIxFor;
                        var offset = rnd.Next(0, 10 - max);
                        getters[optIx] = pairs => ((int[]) pairs[arg].Value)[optIx] + offset;
                    }
                }
                else if (result.Groups[8].Success)
                {
                    switch (result.Groups[8].Value)
                    {
                        case "any module name":
                            var modules = Bomb.GetModuleNames();
                            argGenerators.Add(() => new PhraseArgPair(modules.Count == 0 ? "Regular Crazy Talk" : modules[Rnd.Range(0, modules.Count)], null));
                            break;

                        default:
                            argGenerators.Add(() => new PhraseArgPair("BUG", null));
                            break;
                    }
                }
                phrase = phrase.Substring(result.Index + result.Length);
            }
            phrFmt.Append(phrase);
            rnd.ShuffleFisherYates(getters);

            var phraseFmt = phrFmt.ToString();
            _phraseGenerators[phrIx] = () =>
            {
                PhraseArgPair[] phraseArgs = argGenerators.Select(gen => gen()).ToArray();
                return new PotentialPhraseAction(string.Format(phraseFmt, phraseArgs.Select(pair => pair.Insert).ToArray()), getters.Select(g => g(phraseArgs)).ToArray());
            };
        }
        
        ButtonUp.OnInteract = buttonPress(ButtonUp, -1);
        ButtonDown.OnInteract = buttonPress(ButtonDown, 1);
        ButtonScreen.OnInteract = buttonHold;
        ButtonScreen.OnInteractEnded = buttonRelease;

        ResetModule();
    }

    private static readonly int[] _colDIxs = new[] { 0, 0, 1, 2, 1, 2 };
    private static readonly int[] _colHIxs = new[] { 1, 2, 0, 0, 2, 1 };
    private static readonly int[] _colRIxs = new[] { 2, 1, 2, 1, 0, 0 };
    private static readonly string[] _embellishmentFmts = new[] { "{0}", "It says: {0}", "Quote: {0} End quote", "“{0}”", "It says: “{0}”", "“It says: {0}”" };

    private void ResetModule()
    {
        var phraseIxs = Enumerable.Range(0, _phraseGenerators.Length).ToList().Shuffle();
        _phraseActions = new List<PhraseAction>();
        var already = new HashSet<int>();
        for (int i = 0; i < phraseIxs.Count && already.Count < 5; i++)
        {
            var potential = _phraseGenerators[phraseIxs[i]]();
            var embellishment = Rnd.Range(0, 6);
            var digit = potential.ColValues[_colDIxs[embellishment]];
            if (!already.Add(digit))
                continue;
            _phraseActions.Add(new PhraseAction(string.Format(_embellishmentFmts[embellishment], potential.Phrase), potential.ColValues[_colDIxs[embellishment]], potential.ColValues[_colHIxs[embellishment]], potential.ColValues[_colRIxs[embellishment]]));
        }

        // The last phrase will be the correct one, so leave its digits alone.
        // Shift the digits for the remaining phrases, so they’re all guaranteed wrong because they’re all different
        for (int i = 0; i < _phraseActions.Count - 1; i++)
            _phraseActions[i].ShownDigit = _phraseActions[(i + 1) % (_phraseActions.Count - 1)].ExpectedDigit;
        _phraseActions.Shuffle();

        Debug.LogFormat(@"[Regular Crazy Talk #{0}] Phrases: (D = digit expected from manual, S = digit shown on the display)", _moduleId);
        foreach (var act in _phraseActions)
            Debug.LogFormat(@"[Regular Crazy Talk #{0}] (S={4}, D={1}, H={2}, R={3}, {5}) {6}", _moduleId, act.ExpectedDigit, act.Hold, act.Release, act.ShownDigit, act.ExpectedDigit == act.ShownDigit ? "CORRECT" : "wrong", act.Phrase);
        showPhrase(0);
    }

    private bool buttonHold()
    {
        _timeWhenHeld = (int) Bomb.GetTime() % 10;
        SetWordWrappedText("For the love of — the display just changed, I didn’t know this mod could do that. Does it mention that in the manual?");
        return false;
    }

    private void buttonRelease()
    {
        var curTime = (int) Bomb.GetTime() % 10;

        if (_phraseActions[_selectedPhraseIx].ExpectedDigit != _phraseActions[_selectedPhraseIx].ShownDigit)
            Debug.LogFormat(@"[Regular Crazy Talk #{0}] You held the button on phrase with D={1}/S={2}, which is the wrong phrase.", _moduleId, _phraseActions[_selectedPhraseIx].ExpectedDigit, _phraseActions[_selectedPhraseIx].ShownDigit);
        else if (_timeWhenHeld != _phraseActions[_selectedPhraseIx].Hold)
            Debug.LogFormat(@"[Regular Crazy Talk #{0}] You held the button when the last seconds digit was {1}, but should have been {2}.", _moduleId, _timeWhenHeld, _phraseActions[_selectedPhraseIx].Hold);
        else if (curTime != _phraseActions[_selectedPhraseIx].Release)
            Debug.LogFormat(@"[Regular Crazy Talk #{0}] You released the button when the last seconds digit was {1}, but should have been {2}.", _moduleId, curTime, _phraseActions[_selectedPhraseIx].Release);
        else
            goto correct;

        Module.HandleStrike();
        ResetModule();
        return;

        correct:
        Debug.LogFormat(@"[Regular Crazy Talk #{0}] Button held at time {1} and released at {2}. Module solved.", _moduleId, _timeWhenHeld, curTime);
        TextMesh.gameObject.SetActive(false);
        DigitDisplay.gameObject.SetActive(false);
        Module.HandlePass();
    }

    private void showPhrase(int ix)
    {
        _selectedPhraseIx = ix;
        SetWordWrappedText(_phraseActions[_selectedPhraseIx].Phrase);
        DigitDisplay.text = _phraseActions[_selectedPhraseIx].ShownDigit.ToString();
    }

    private KMSelectable.OnInteractHandler buttonPress(KMSelectable button, int offset)
    {
        return delegate
        {
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, button.transform);
            button.AddInteractionPunch(.25f);
            if (!_isSolved)
                showPhrase((_selectedPhraseIx + offset + _phraseActions.Count) % _phraseActions.Count);
            return false;
        };
    }

    private void SetWordWrappedText(string text)
    {
        var acceptableWidth = 80d;
        var desiredHeight = 117d;

        var low = 1;
        var high = 192;
        var wrappeds = new Dictionary<int, string>();

        TextMesh.transform.SetParent(null, false);
        TextMesh.transform.localPosition = new Vector3(0, 0, 0);
        TextMesh.transform.localRotation = Quaternion.Euler(90, 0, 0);
        TextMesh.transform.localScale = new Vector3(1, 1, 1);

        while (high - low > 1)
        {
            var mid = (low + high) / 2;
            TextMesh.fontSize = mid;

            TextMesh.text = "\u00a0";
            var widthOfASpace = TextRenderer.bounds.size.x;

            var wrappedSB = new StringBuilder();
            var first = true;
            foreach (var line in Ut.WordWrap(
                text,
                line => acceptableWidth,
                widthOfASpace,
                str =>
                {
                    TextMesh.text = str;
                    return TextRenderer.bounds.size.x;
                },
                allowBreakingWordsApart: false
            ))
            {
                if (line == null)
                {
                    // There was a word that was too long to fit into a line.
                    high = mid;
                    wrappedSB = null;
                    break;
                }
                if (!first)
                    wrappedSB.Append('\n');
                first = false;
                wrappedSB.Append(line);
            }

            if (wrappedSB != null)
            {
                var wrapped = wrappedSB.ToString();
                wrappeds[mid] = wrapped;
                TextMesh.text = wrapped;
                if (TextRenderer.bounds.size.z > desiredHeight)
                    high = mid;
                else
                    low = mid;
            }
        }

        TextMesh.fontSize = low;
        TextMesh.text = wrappeds[low];

        TextMesh.transform.SetParent(TextMeshParent, false);
        TextMesh.transform.localPosition = new Vector3(-0.083f, 0.0002f, 0.12f);
        TextMesh.transform.localRotation = Quaternion.Euler(90, 0, 0);
        TextMesh.transform.localScale = new Vector3(.002f, .002f, .002f);
    }

#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} up | !{0} down | !{0} toggle 2 4 [hold on last digit 2, release on 4]";
#pragma warning restore 414

    private IEnumerator ProcessTwitchCommand(string command)
    {
        Match match;

        if (Regex.IsMatch(command, @"^\s*up\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            yield return new[] { ButtonUp };
        }
        else if (Regex.IsMatch(command, @"^\s*down\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            yield return new[] { ButtonDown };
        }
        else if ((match = Regex.Match(command, @"^\s*(?:hold|release|toggle)(?:\s+at)?\s+(\d)\s*(\d)\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)).Success)
        {
            yield return null;
            yield return "solve";
            yield return "strike";
            var digit1 = int.Parse(match.Groups[1].Value);
            while ((int) Bomb.GetTime() % 10 != digit1)
                yield return "trycancel";
            yield return ButtonScreen;
            var digit2 = int.Parse(match.Groups[2].Value);
            while ((int) Bomb.GetTime() % 10 != digit2)
                yield return null;  // don’t let this be canceled because then it would remain held
            yield return ButtonScreen;
        }
    }
}
