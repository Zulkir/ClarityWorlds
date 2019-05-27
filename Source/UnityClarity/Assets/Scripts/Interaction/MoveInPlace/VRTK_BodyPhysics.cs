using Assets.Scripts.Helpers;
using Clarity.App.Worlds.StoryGraph.FreeNavigation;
using UnityEngine;
using ca = Clarity.Common.Numericals.Algebra;

namespace Assets.Scripts.Interaction.MoveInPlace
{
    public class VRTK_BodyPhysics
    {
        private readonly float radius;
        private readonly ICollisionMesh collisionMesh;
        private bool isFalling;

        public VRTK_BodyPhysics(ICollisionMesh collisionMesh, float radius)
        {
            this.collisionMesh = collisionMesh;
            this.radius = radius;
        }

        public void SetIsFalling(bool value) => isFalling = value;
        public bool IsFalling() => isFalling;

        public bool SweepCollision(Vector3 currentPosition, Vector3 proposedDirection, float distance)
        {
            proposedDirection.y = 0;
            proposedDirection.Normalize();
            var offset = (proposedDirection * distance).ToClarity();
            var currentPosClarity = currentPosition.ToClarity() + 1 * ca.Vector3.UnitY;
            var expectedTarget = currentPosClarity + 1 * ca.Vector3.UnitY + offset;
            var target = collisionMesh.RestrictMovement(radius, currentPosClarity, offset);
            return target.Xz != expectedTarget.Xz;
        }
    }
}