using System;
using Reflex.Attributes;
using UnityEngine;

namespace CrashyChasy.Game.DeathScreen
{
    public sealed class DeathScreenLifetime : MonoBehaviour
    {
        [Inject] private readonly DeathScreenPresenter _presenter;

        private void OnEnable()
        {
            _presenter.Subscribe();
        }

        private void OnDisable()
        {
            _presenter.Unsubscribe();
        }
    }
}