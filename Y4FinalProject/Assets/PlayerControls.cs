//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/PlayerControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Player Movement"",
            ""id"": ""2ba272f0-b831-483a-a48e-eaa09b4d0c26"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""8f5db489-e30d-4703-9910-1871a327817f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""4c194b02-dd2a-4b8c-a354-c718932145f4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Vault"",
                    ""type"": ""Button"",
                    ""id"": ""4980f477-feec-443e-b2e2-2690a1703099"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Reset"",
                    ""type"": ""Button"",
                    ""id"": ""42605abb-a188-4fe1-8afb-a942ffd4a2ab"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0d8166e9-82f8-48e2-9747-8f52b34bde0b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ControlKey"",
                    ""type"": ""Button"",
                    ""id"": ""b422c984-2065-4cb0-a5fd-066703c315f8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SpeedKey"",
                    ""type"": ""Button"",
                    ""id"": ""3d6a7815-4b63-4148-97bb-4d16828c1009"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Ability"",
                    ""type"": ""Button"",
                    ""id"": ""de9af29c-99f8-4919-9d9c-c206beb8672a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Change Ability"",
                    ""type"": ""Button"",
                    ""id"": ""88d142ae-30eb-4cbc-a224-2a984e43bda3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""50e38886-c8ae-4cb7-82c2-0439a9a0b0cc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Reel"",
                    ""type"": ""Button"",
                    ""id"": ""c3529cec-4b32-46a4-a003-8610ffaec684"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Grab"",
                    ""type"": ""Button"",
                    ""id"": ""a1931224-a423-4f21-88a8-3375da53bde6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""eb9d3ae4-0124-426f-86c2-3ede8a1fdea8"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e7b232fa-1e71-4bae-a4a7-912852ddfdde"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d81f198f-4e89-4846-90da-7a15a66dc06c"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6dd36c33-5071-480c-850e-990b77f9333a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1796dd22-656b-4903-bd84-dc6a65bf8c3b"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5939b673-e3f1-4dc2-ad46-a4f68e6c0b1d"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""08fce257-2a27-470f-81e5-60c2eaf81a7e"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c72f4d47-85f0-4766-8c68-c326dac8db69"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""697ce30b-b87d-475d-881d-085f26b311b1"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Vault"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ea51081-a142-44e6-8c70-6421b48b779e"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Vault"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c07e9cd-bd4c-4206-b8bf-3b7a179ce0d9"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Reset"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f6751d4-63d4-4e97-b23e-22901ba638bc"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Reset"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8b629670-10cf-4ed8-b4f1-dcf79452bc30"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a2ff288f-2fbf-4933-9583-6e59611ee38f"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpeedKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""21b3b33a-f1ed-414e-975b-58f8cd760364"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpeedKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""56bb0e71-b314-4879-95ff-05c9fe15b48b"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ControlKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""851cf09c-6e9e-4d36-984c-215abc0631f4"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ControlKey"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bdafa9e3-787d-4ab5-8604-bc8fd3933ba2"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ability"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8394950b-fcd6-4ee7-aada-9bf56cf2f85e"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Change Ability"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""da227d87-d8bb-4134-acad-fc6b2c142b75"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""ef7afe5b-5b03-4752-b8eb-4c211e9d21ed"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reel"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""b54eb3a6-1eb2-4915-b8d6-e06aa4579549"",
                    ""path"": ""<Mouse>/scroll/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""ebd63ce7-5c78-42e0-b8fc-79673a57c8b3"",
                    ""path"": ""<Mouse>/scroll/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""13c15136-1b3e-43c7-a354-7c0bc736baac"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Grab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""bb"",
            ""id"": ""35ed2833-8d55-4789-af05-084aae760046"",
            ""actions"": [
                {
                    ""name"": ""Reset Timer"",
                    ""type"": ""Button"",
                    ""id"": ""685d7eff-6862-47e5-98d1-49b4f147ba2a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4079c518-9e1a-4a28-8690-70588353a0f7"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Reset Timer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": []
        },
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": []
        }
    ]
}");
        // Player Movement
        m_PlayerMovement = asset.FindActionMap("Player Movement", throwIfNotFound: true);
        m_PlayerMovement_Movement = m_PlayerMovement.FindAction("Movement", throwIfNotFound: true);
        m_PlayerMovement_Jump = m_PlayerMovement.FindAction("Jump", throwIfNotFound: true);
        m_PlayerMovement_Vault = m_PlayerMovement.FindAction("Vault", throwIfNotFound: true);
        m_PlayerMovement_Reset = m_PlayerMovement.FindAction("Reset", throwIfNotFound: true);
        m_PlayerMovement_Look = m_PlayerMovement.FindAction("Look", throwIfNotFound: true);
        m_PlayerMovement_ControlKey = m_PlayerMovement.FindAction("ControlKey", throwIfNotFound: true);
        m_PlayerMovement_SpeedKey = m_PlayerMovement.FindAction("SpeedKey", throwIfNotFound: true);
        m_PlayerMovement_Ability = m_PlayerMovement.FindAction("Ability", throwIfNotFound: true);
        m_PlayerMovement_ChangeAbility = m_PlayerMovement.FindAction("Change Ability", throwIfNotFound: true);
        m_PlayerMovement_Pause = m_PlayerMovement.FindAction("Pause", throwIfNotFound: true);
        m_PlayerMovement_Reel = m_PlayerMovement.FindAction("Reel", throwIfNotFound: true);
        m_PlayerMovement_Grab = m_PlayerMovement.FindAction("Grab", throwIfNotFound: true);
        // bb
        m_bb = asset.FindActionMap("bb", throwIfNotFound: true);
        m_bb_ResetTimer = m_bb.FindAction("Reset Timer", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player Movement
    private readonly InputActionMap m_PlayerMovement;
    private List<IPlayerMovementActions> m_PlayerMovementActionsCallbackInterfaces = new List<IPlayerMovementActions>();
    private readonly InputAction m_PlayerMovement_Movement;
    private readonly InputAction m_PlayerMovement_Jump;
    private readonly InputAction m_PlayerMovement_Vault;
    private readonly InputAction m_PlayerMovement_Reset;
    private readonly InputAction m_PlayerMovement_Look;
    private readonly InputAction m_PlayerMovement_ControlKey;
    private readonly InputAction m_PlayerMovement_SpeedKey;
    private readonly InputAction m_PlayerMovement_Ability;
    private readonly InputAction m_PlayerMovement_ChangeAbility;
    private readonly InputAction m_PlayerMovement_Pause;
    private readonly InputAction m_PlayerMovement_Reel;
    private readonly InputAction m_PlayerMovement_Grab;
    public struct PlayerMovementActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerMovementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerMovement_Movement;
        public InputAction @Jump => m_Wrapper.m_PlayerMovement_Jump;
        public InputAction @Vault => m_Wrapper.m_PlayerMovement_Vault;
        public InputAction @Reset => m_Wrapper.m_PlayerMovement_Reset;
        public InputAction @Look => m_Wrapper.m_PlayerMovement_Look;
        public InputAction @ControlKey => m_Wrapper.m_PlayerMovement_ControlKey;
        public InputAction @SpeedKey => m_Wrapper.m_PlayerMovement_SpeedKey;
        public InputAction @Ability => m_Wrapper.m_PlayerMovement_Ability;
        public InputAction @ChangeAbility => m_Wrapper.m_PlayerMovement_ChangeAbility;
        public InputAction @Pause => m_Wrapper.m_PlayerMovement_Pause;
        public InputAction @Reel => m_Wrapper.m_PlayerMovement_Reel;
        public InputAction @Grab => m_Wrapper.m_PlayerMovement_Grab;
        public InputActionMap Get() { return m_Wrapper.m_PlayerMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerMovementActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerMovementActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerMovementActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerMovementActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Vault.started += instance.OnVault;
            @Vault.performed += instance.OnVault;
            @Vault.canceled += instance.OnVault;
            @Reset.started += instance.OnReset;
            @Reset.performed += instance.OnReset;
            @Reset.canceled += instance.OnReset;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @ControlKey.started += instance.OnControlKey;
            @ControlKey.performed += instance.OnControlKey;
            @ControlKey.canceled += instance.OnControlKey;
            @SpeedKey.started += instance.OnSpeedKey;
            @SpeedKey.performed += instance.OnSpeedKey;
            @SpeedKey.canceled += instance.OnSpeedKey;
            @Ability.started += instance.OnAbility;
            @Ability.performed += instance.OnAbility;
            @Ability.canceled += instance.OnAbility;
            @ChangeAbility.started += instance.OnChangeAbility;
            @ChangeAbility.performed += instance.OnChangeAbility;
            @ChangeAbility.canceled += instance.OnChangeAbility;
            @Pause.started += instance.OnPause;
            @Pause.performed += instance.OnPause;
            @Pause.canceled += instance.OnPause;
            @Reel.started += instance.OnReel;
            @Reel.performed += instance.OnReel;
            @Reel.canceled += instance.OnReel;
            @Grab.started += instance.OnGrab;
            @Grab.performed += instance.OnGrab;
            @Grab.canceled += instance.OnGrab;
        }

        private void UnregisterCallbacks(IPlayerMovementActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Vault.started -= instance.OnVault;
            @Vault.performed -= instance.OnVault;
            @Vault.canceled -= instance.OnVault;
            @Reset.started -= instance.OnReset;
            @Reset.performed -= instance.OnReset;
            @Reset.canceled -= instance.OnReset;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @ControlKey.started -= instance.OnControlKey;
            @ControlKey.performed -= instance.OnControlKey;
            @ControlKey.canceled -= instance.OnControlKey;
            @SpeedKey.started -= instance.OnSpeedKey;
            @SpeedKey.performed -= instance.OnSpeedKey;
            @SpeedKey.canceled -= instance.OnSpeedKey;
            @Ability.started -= instance.OnAbility;
            @Ability.performed -= instance.OnAbility;
            @Ability.canceled -= instance.OnAbility;
            @ChangeAbility.started -= instance.OnChangeAbility;
            @ChangeAbility.performed -= instance.OnChangeAbility;
            @ChangeAbility.canceled -= instance.OnChangeAbility;
            @Pause.started -= instance.OnPause;
            @Pause.performed -= instance.OnPause;
            @Pause.canceled -= instance.OnPause;
            @Reel.started -= instance.OnReel;
            @Reel.performed -= instance.OnReel;
            @Reel.canceled -= instance.OnReel;
            @Grab.started -= instance.OnGrab;
            @Grab.performed -= instance.OnGrab;
            @Grab.canceled -= instance.OnGrab;
        }

        public void RemoveCallbacks(IPlayerMovementActions instance)
        {
            if (m_Wrapper.m_PlayerMovementActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerMovementActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerMovementActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerMovementActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerMovementActions @PlayerMovement => new PlayerMovementActions(this);

    // bb
    private readonly InputActionMap m_bb;
    private List<IBbActions> m_BbActionsCallbackInterfaces = new List<IBbActions>();
    private readonly InputAction m_bb_ResetTimer;
    public struct BbActions
    {
        private @PlayerControls m_Wrapper;
        public BbActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @ResetTimer => m_Wrapper.m_bb_ResetTimer;
        public InputActionMap Get() { return m_Wrapper.m_bb; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BbActions set) { return set.Get(); }
        public void AddCallbacks(IBbActions instance)
        {
            if (instance == null || m_Wrapper.m_BbActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_BbActionsCallbackInterfaces.Add(instance);
            @ResetTimer.started += instance.OnResetTimer;
            @ResetTimer.performed += instance.OnResetTimer;
            @ResetTimer.canceled += instance.OnResetTimer;
        }

        private void UnregisterCallbacks(IBbActions instance)
        {
            @ResetTimer.started -= instance.OnResetTimer;
            @ResetTimer.performed -= instance.OnResetTimer;
            @ResetTimer.canceled -= instance.OnResetTimer;
        }

        public void RemoveCallbacks(IBbActions instance)
        {
            if (m_Wrapper.m_BbActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IBbActions instance)
        {
            foreach (var item in m_Wrapper.m_BbActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_BbActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public BbActions @bb => new BbActions(this);
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface IPlayerMovementActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnVault(InputAction.CallbackContext context);
        void OnReset(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnControlKey(InputAction.CallbackContext context);
        void OnSpeedKey(InputAction.CallbackContext context);
        void OnAbility(InputAction.CallbackContext context);
        void OnChangeAbility(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnReel(InputAction.CallbackContext context);
        void OnGrab(InputAction.CallbackContext context);
    }
    public interface IBbActions
    {
        void OnResetTimer(InputAction.CallbackContext context);
    }
}
