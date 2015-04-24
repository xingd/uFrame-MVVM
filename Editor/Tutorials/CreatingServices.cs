using Invert.Core.GraphDesigner;
using Invert.StateMachine;
using Invert.uFrame.MVVM;

public class CreatingServices : uFrameMVVMTutorial
{
    public override decimal Order
    {
        get { return 3; }
    }


    protected override void TutorialContent(IDocumentationBuilder _)
    {
        BasicSetup(_);

        CreateGameElement(_);
        CreateGameView(_);

        // var debugService = DoNamedNodeStep<ServiceNode>(_, "DebugService");
        var graph = DoGraphStep<ServiceGraph>(_,null, b => { });
        var debugService = graph == null ? null : graph.RootFilter as ServiceNode;
        var logEvent = DoNamedNodeStep<SimpleClassNode>(_, "LogEvent");
        DoNamedItemStep<PropertiesChildItem>(_, "Message", logEvent, "a property", b => { });
        DoNamedItemStep<HandlersReference>(_, "LogEvent", debugService, "a handler", b => { });
        SaveAndCompile(_);
        EnsureCode(_, debugService, "Open DebugService.cs and implement the LogEventHandler method.", "http://i.imgur.com/Vrdqgx4.png", "DebugService", "Debug.Log");
        EnsureKernel(_);
    }

    protected override void Introduction(IDocumentationBuilder _)
    {
        
    }

    protected override void Ending(IDocumentationBuilder _, InteractiveTutorial tutorial)
    {
        _.ImageByUrl("http://i.imgur.com/iprda4t.png");
        _.Paragraph("Now run the game from SceneA, select the gameobject and hit the LoadB button and UnLoadB button from the GameView's inspector.");
    }
}

public class UsingStateMachines : uFrameMVVMTutorial
{
    protected override void TutorialContent(IDocumentationBuilder _)
    {
        BasicSetup(_);
        CreateGameElement(_);
        CreateGameView(_);

        //StateGraph = DoGraphStep<StateMachineGraph>(_, "GameFlow");
        StateMachineNode = DoNamedNodeStep<StateMachineNode>(_, "GameFlow", TheGame);

        BeginGameTransition = DoNamedItemStep<TransitionsChildItem>(_, "BeginGame", StateMachineNode, "a transition", b =>
        {
            
            b.Paragraph("We need to register a transition to the state machine node.");
            b.ImageByUrl("http://i.imgur.com/swPtoys.png");
        });

        PlayCommand = DoNamedItemStep<CommandsChildItem>(_, "Play", TheGame, "a command", b =>
        {
            b.Paragraph("Now we need to add a command that will trigger the transition.");
        });

        DoCreateConnectionStep(_, PlayCommand, BeginGameTransition, b =>
        {

            b.Paragraph("Creating this connection means that when the command is invoked it will trigger the transition automatically.");
            b.ImageByUrl("http://i.imgur.com/LEMP06i.png");
        });

        StateMachineProperty = DoNamedItemStep<PropertiesChildItem>(_, "GameFlow", TheGame, "a property", b =>
        {
            b.Paragraph("State machines live on the view-model. So we need to create a property on our element node for the state machine.");
        });
        DoCreateConnectionStep(_, StateMachineProperty, StateMachineNode, b =>
        {
            b.ImageByUrl("http://i.imgur.com/LEMP06i.png");
        });
        //StateGraph == null ? null : StateGraph.RootFilter as StateMachineNode;
        MainMenuState = DoNamedNodeStep<StateNode>(_, "MainMenu",StateMachineNode);
        PlayingGameState = DoNamedNodeStep<StateNode>(_, "PlayingGame", StateMachineNode);

      
        BeginGameStateTransition = DoNamedItemStep<StateTransitionsReference>(_, "BeginGame", MainMenuState, "a state transition", b =>
        {
            b.Paragraph("Now we need to add the transition to the MainMenu state.");
        });
        DoCreateConnectionStep(_, StateMachineNode == null ? null : StateMachineNode.StartStateOutputSlot, MainMenuState);

        DoCreateConnectionStep(_, BeginGameStateTransition, 
            PlayingGameState
            );

        DoNamedItemStep<BindingsReference>(_, "GameFlow State Changed", GameView, "a binding", b =>
        {
            b.ImageByUrl("http://i.imgur.com/uHAfHEM.png");

        });

        SaveAndCompile(_);
        EnsureKernel(_);
        AddViewToScene(_,GameView);
    }

    public StateTransitionsReference BeginGameStateTransition { get; set; }

    public PropertiesChildItem StateMachineProperty { get; set; }

    public CommandsChildItem PlayCommand { get; set; }

    public TransitionsChildItem BeginGameTransition { get; set; }

    public StateMachineNode StateMachineNode { get; set; }

    public StateNode PlayingGameState { get; set; }

    public StateNode MainMenuState { get; set; }

    public StateMachineGraph StateGraph { get; set; }

    protected override void Introduction(IDocumentationBuilder _)
    {
        
    }

    protected override void Ending(IDocumentationBuilder _, InteractiveTutorial tutorial)
    {
        
    }
}