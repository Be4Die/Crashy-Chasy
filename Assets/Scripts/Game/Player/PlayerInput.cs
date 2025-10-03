using UnityEngine;

namespace CrashyChasy.Game.Player
{
    public sealed class PlayerInput
    {
        private int _turnDirection;
        
        private readonly UnityEngine.Camera _camera;

        public PlayerInput(UnityEngine.Camera camera)
        {
            _camera = camera;
        }
        

        public PlayerInputData Handle()
        {
            ResetInput();
            ProcessTouchInput();
            ProcessKeyboardInput();

            return new PlayerInputData
            {
                TurnDirection = _turnDirection
            };
        }

        private void ResetInput()
        {
            _turnDirection = 0;
        }

        private void ProcessTouchInput()
        {
            if (Input.touchCount > 0)
            {
                foreach (var touch in Input.touches)
                {
                    if (touch.phase != TouchPhase.Began && touch.phase != TouchPhase.Stationary &&
                        touch.phase != TouchPhase.Moved) continue;
                    EvaluateTouchPosition(touch.position);
                    return;
                }
            }
            else if (Input.GetMouseButton(0))
            {
                EvaluateTouchPosition(Input.mousePosition);
            }
        }

        private void EvaluateTouchPosition(Vector2 inputPosition)
        {
            Vector2 viewportPosition = _camera.ScreenToViewportPoint(inputPosition);
            _turnDirection = viewportPosition.x <= 0.5f ? -1 : 1;
        }

        private void ProcessKeyboardInput()
        {
            if (_turnDirection != 0) return;
            var horizontal = Input.GetAxisRaw("Horizontal");
            if (horizontal == 0) return;
            _turnDirection = horizontal > 0 ? 1 : -1;
        }
    }
}